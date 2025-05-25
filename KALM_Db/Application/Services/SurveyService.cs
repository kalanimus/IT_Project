using Application.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ISurveyAnswerRepository _surveyAnswerRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;
        private readonly IGroupTeacherRepository _groupTeacherRepository;
        private readonly IMistralService _mistralService;

        public SurveyService(
            ISurveyRepository surveyRepository,
            ISurveyAnswerRepository surveyAnswerRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IGroupStudentRepository groupStudentRepository,
            IGroupTeacherRepository groupTeacherRepository,
            IMistralService mistralService)
        {
            _surveyRepository = surveyRepository;
            _surveyAnswerRepository = surveyAnswerRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _groupStudentRepository = groupStudentRepository;
            _groupTeacherRepository = groupTeacherRepository;
            _mistralService = mistralService;
        }

        public async Task<ModelSurvey> GetByIdAsync(int id)
        {
            return await _surveyRepository.GetByIdAsync(id);
        }

        public async Task<List<ModelSurvey>> GetByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);
            if (user != null)
            {
                if (user.Role.RoleName == "Студент")
                {
                    var group = await _groupStudentRepository.GetByUsernameAsync(userName);
                    var teachers = await _groupTeacherRepository.GetByGroupIdAsync(group.GroupId);
                    var standartSurvey = await _surveyRepository.GetStandartAsync();

                    var surveys = teachers.Select(t => new ModelSurvey
                    {
                        Id = standartSurvey.Id,
                        Title = $"{standartSurvey.Title} ({t.Subject.SubjectName})",
                        Description = standartSurvey.Description,
                        IsStandart = true,
                        Teacher = t,
                        QuestionsJson = standartSurvey.QuestionsJson
                    }).ToList();
                    surveys.AddRange(await _surveyRepository.GetByGroupAsync(group.GroupId));
                    return surveys;
                }
                else if (user.Role.RoleName == "Преподаватель")
                {
                    var groups = await _groupTeacherRepository.GetGroupTeachersByIdAsync(user.Id);

                    var standartSurvey = await _surveyRepository.GetStandartAsync();
                    var surveys = groups.Select(t => new ModelSurvey
                    {
                        Id = standartSurvey.Id,
                        Title = $"{standartSurvey.Title} ({t.Subject.SubjectName})",
                        Description = null,
                        IsStandart = true,
                        Teacher = t,
                        QuestionsJson = standartSurvey.QuestionsJson
                    }).ToList();
                    surveys.AddRange(await _surveyRepository.GetByUserNameAsync(userName));
                    return surveys;
                }
                else throw new ForbiddenException("Wrong role", "Неверная роль");
            }
            else throw new UserNotFoundException();
        }


        public async Task<List<ModelSurvey>> GetAllAsync()
        {
            return await _surveyRepository.GetAllAsync();
        }

        public async Task AddAsync(ModelSurvey survey)
        {
            await _surveyRepository.AddAsync(survey);
        }

        public async Task UpdateAsync(ModelSurvey survey)
        {
            var existingSurvey = await _surveyRepository.GetByIdAsync(survey.Id);
            if (existingSurvey == null) throw new Exception("Survey not found");

            // Обновляем поля существующей роли
            existingSurvey.Title = survey.Title;
            existingSurvey.Description = survey.Description;
            existingSurvey.IsStandart = survey.IsStandart;
            existingSurvey.QuestionsJson = survey.QuestionsJson;
            existingSurvey.Results = survey.Results;
            // Другие поля, если необходимо

            await _surveyRepository.UpdateAsync(existingSurvey);
        }

        public async Task DeleteAsync(int id)
        {
            var survey = await _surveyRepository.GetByIdAsync(id);
            if (survey == null) throw new Exception("Survey not found");

            await _surveyRepository.DeleteAsync(survey.Id);
        }

        public async Task<ModelSurveyAnalytics> GetAnalyticsAsync(int surveyId, string groupName, string subjectName, string userName)
        {
            var survey = await _surveyRepository.GetByIdAsync(surveyId);
            if (survey == null) throw new Exception("Survey not found");
            var teacher = await _userRepository.GetByUsernameAsync(userName);
            var surveyAnswers = _mapper.Map<List<SurveyAnswerDto>>(await _surveyAnswerRepository.GetBySurveyDetailsAsync(surveyId, subjectName, teacher.FullName, groupName));
            if (survey.IsStandart)
            {
                return await MakeStandartAnalyticsAsync(surveyAnswers, surveyId);
            }
            else
            {
                return MakeCustomAnalytics(surveyAnswers);
                return null;
            }
        }

        private ModelSurveyAnalytics MakeCustomAnalytics(List<SurveyAnswerDto> surveyAnswers)
        {
            var analytics = new ModelSurveyAnalytics
            {
                Params = new List<ModelAnswerParam>()
            };

            // Группируем ответы по тексту вопроса
            var questionGroups = surveyAnswers
                .SelectMany(answer => answer.Answers)
                .GroupBy(q => new { q.Question, q.QuestionType }); // Предполагается, что есть QuestionType

            foreach (var group in questionGroups)
            {
                var questionText = group.Key.Question;
                var questionType = group.Key.QuestionType?.ToLower();

                if (questionType == "single_choice" || questionType == "multiple_choice")
                {
                    // Считаем количество каждого варианта ответа
                    var answerCounts = group
                        .SelectMany(q =>
                            questionType == "multiple_choice" && q.Answer.Contains(";")
                                ? q.Answer.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                : new[] { q.Answer }
                        )
                        .GroupBy(ans => ans.Trim())
                        .ToDictionary(g => g.Key, g => g.Count());

                    analytics.Params.Add(new ModelAnswerParam
                    {
                        Param = questionText,
                        Count = group.Count(),
                        AnswerCounts = answerCounts
                    });
                }
                else if (questionType == "text")
                {
                    // Собираем все текстовые ответы
                    var textAnswers = group.Select(q => q.Answer).ToList();

                    analytics.Params.Add(new ModelAnswerParam
                    {
                        Param = questionText,
                        Count = textAnswers.Count,
                        TextAnswers = textAnswers
                    });
                }
                else
                {
                    // Если тип не определён, просто считаем как текстовые
                    var textAnswers = group.Select(q => q.Answer).ToList();

                    analytics.Params.Add(new ModelAnswerParam
                    {
                        Param = questionText,
                        Count = textAnswers.Count,
                        TextAnswers = textAnswers
                    });
                }
            }

            return analytics;
        }

        public async Task<ModelSurveyAnalytics> MakeStandartAnalyticsAsync(
            List<SurveyAnswerDto> surveyAnswers,
            int surveyId)
        {
            var analytics = new ModelSurveyAnalytics
            {
                SurveyId = surveyId,
                Params = new List<ModelAnswerParam>()
            };

            var questionGroups = surveyAnswers
                .SelectMany(answer => answer.Answers)
                .GroupBy(q => new { q.Question, q.QuestionType });

            List<string> openAnswers = new();

            foreach (var group in questionGroups)
            {
                var questionText = group.Key.Question;
                var questionType = group.Key.QuestionType.ToString();

                if (questionType == "single_choice")
                {
                    var scores = group
                        .Select(q => int.TryParse(q.Answer, out var score) ? (int?)score : null)
                        .Where(score => score.HasValue)
                        .Select(score => score.Value)
                        .ToList();

                    analytics.Params.Add(new ModelAnswerParam
                    {
                        Param = questionText,
                        QuestionType = questionType,
                        Count = scores.Count,
                        Average = scores.Any() ? scores.Average() : 0
                    });
                }
                else if (questionType == "text")
                {
                    var textAnswers = group
                        .Select(q => q.Answer?.Trim())
                        .Where(a => !string.IsNullOrWhiteSpace(a) && a.Any(char.IsLetter))
                        .ToList();

                    if (textAnswers.Any())
                    {
                        analytics.Params.Add(new ModelAnswerParam
                        {
                            Param = questionText,
                            QuestionType = questionType,
                            Count = textAnswers.Count,
                            TextAnswers = textAnswers
                        });
                        openAnswers.AddRange(textAnswers);
                    }
                }
            }

            // Генерация общего комментария через Мистраль
            if (openAnswers.Any())
            {
                var prompt = "На основе следующих комментариев студентов составь общий вывод или рекомендацию для преподавателя. Не используй нецензурные выражения. Комментарии:\n";
                prompt += string.Join("\n- ", openAnswers);

                analytics.GeneralComment = await _mistralService.SendPromptAsync(prompt);
            }

            return analytics;
        }

    }
}