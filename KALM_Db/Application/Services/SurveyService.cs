using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupStudentRepository _groupStudentRepository;

        public SurveyService(
            ISurveyRepository surveyRepository,
            IUserRepository userRepository,
            IGroupStudentRepository groupStudentRepository)
        {
            _surveyRepository = surveyRepository;
            _userRepository = userRepository;
            _groupStudentRepository = groupStudentRepository;
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
                    return await _surveyRepository.GetByGroupAsync(group.GroupId);
                } else if (user.Role.RoleName == "Преподаватель")
                {
                    return await _surveyRepository.GetByUserNameAsync(userName);
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
    }
}