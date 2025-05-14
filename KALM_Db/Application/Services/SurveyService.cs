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

        public SurveyService(
            ISurveyRepository surveyRepository,
            ISurveyAnswerRepository surveyAnswerRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IGroupStudentRepository groupStudentRepository,
            IGroupTeacherRepository groupTeacherRepository)
        {
            _surveyRepository = surveyRepository;
            _surveyAnswerRepository = surveyAnswerRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _groupStudentRepository = groupStudentRepository;
            _groupTeacherRepository = groupTeacherRepository;
        }

        public async Task<ModelSurvey> GetByIdAsync(int id)
        {
            return await _surveyRepository.GetByIdAsync(id);
        }

        public async Task<List<ModelSurvey>> GetByUserNameAsync(string userName){
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
                } else if (user.Role.RoleName == "Преподаватель")
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
                        QuestionsJson = null
                    }).ToList();
                    surveys.AddRange(await _surveyRepository.GetByUserNameAsync(userName));
                    return surveys;
                } else throw new ForbiddenException("Wrong role", "Неверная роль");
            } else throw new UserNotFoundException();
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

        public async Task<ModelSurveyAnalytics> GetAnalyticsAsync(int surveyId, string userName)
        {
            var survey = await _surveyRepository.GetByIdAsync(surveyId);
            if (survey == null) throw new Exception("Survey not found");
            var surveyAnswers = _mapper.Map<List<SurveyAnswerDto>>(await _surveyAnswerRepository.GetBySurveyIdAsync(surveyId));
            if (survey.IsStandart)
            {
                await MakeStandartAnalyticsAsync(surveyAnswers);
            }
            return null;

            
        }

        public async Task<ModelSurveyAnalytics> MakeStandartAnalyticsAsync(List<SurveyAnswerDto> surveyAnswers)
        {
            var analytics = new ModelSurveyAnalytics
            {
                Params = new List<ModelAnswerParam>()
            };

            // Группируем ответы по тексту вопроса
            var questionGroups = surveyAnswers
                .SelectMany(answer => answer.Answers) // Разворачиваем все ответы
                .GroupBy(question => question.Question); // Группируем по тексту вопроса

            foreach (var group in questionGroups)
            {
                var questionText = group.Key;
                var scores = group
                    .Select(q =>
                    {
                        if (int.TryParse(q.Answer, out var score))
                        {
                            return score; // Успешно преобразованное значение
                        }
                        return (int?)null; // Если преобразование не удалось
                    })
                    .Where(score => score.HasValue) // Исключаем некорректные значения
                    .Select(score => score.Value) // Преобразуем в int
                    .ToList();

                // Подсчитываем количество ответов и среднее арифметическое
                var count = scores.Count;
                var average = scores.Any() ? scores.Average() : 0;

                // Добавляем данные в аналитику
                analytics.Params.Add(new ModelAnswerParam
                {
                    Param = questionText,
                    Count = count,
                    Average = average
                });
            }

            return analytics;
        }
        
    }
}