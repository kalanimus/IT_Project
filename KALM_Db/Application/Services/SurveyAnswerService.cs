using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class SurveyAnswerService : ISurveyAnswerService
    {
        private readonly ISurveyAnswerRepository _surveyAnswerRepository;
        private readonly IUserRepository _userRepository;
        private readonly int _surveyAnswerPrice;

        public SurveyAnswerService(
            ISurveyAnswerRepository surveyAnswerRepository,
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _surveyAnswerRepository = surveyAnswerRepository;
            _userRepository = userRepository;
            _surveyAnswerPrice = configuration.GetValue<int>("Constants:SurveyAnswerPrice");
        }

        public async Task<ModelSurveyAnswer> GetByIdAsync(int id)
        {
            return await _surveyAnswerRepository.GetByIdAsync(id);
        }

        public async Task<List<ModelSurveyAnswer>> GetAllAsync()
        {
            return await _surveyAnswerRepository.GetAllAsync();
        }

        public async Task AddAsync(ModelSurveyAnswer surveyAnswer)
        {
            await _surveyAnswerRepository.AddAsync(surveyAnswer);

            var author = await _userRepository.GetByUsernameAsync(surveyAnswer.AuthorUsername);
            
        }

        public async Task UpdateAsync(ModelSurveyAnswer surveyAnswer)
        {
            var existingSurveyAnswer = await _surveyAnswerRepository.GetByIdAsync(surveyAnswer.Id);
            if (existingSurveyAnswer == null) throw new Exception("Survey not found");

            // Обновляем поля существующей роли
            existingSurveyAnswer.SurveyId = surveyAnswer.SurveyId;
            existingSurveyAnswer.Survey = surveyAnswer.Survey;
            existingSurveyAnswer.AnswerJson = surveyAnswer.AnswerJson;
            // Другие поля, если необходимо

            await _surveyAnswerRepository.UpdateAsync(existingSurveyAnswer);
        }

        public async Task DeleteAsync(int id)
        {
            var surveyAnswer = await _surveyAnswerRepository.GetByIdAsync(id);
            if (surveyAnswer == null) throw new Exception("Survey not found");

            await _surveyAnswerRepository.DeleteAsync(surveyAnswer.Id);
        }

        public async Task<bool> IsSurveyCompletedAsync(int id, string teacherFullName, string subject, string userName)
        {
            var surveyAnswer = await _surveyAnswerRepository.GetByDetails(id, teacherFullName, subject, userName);
            return surveyAnswer != null;
        }
    }
}