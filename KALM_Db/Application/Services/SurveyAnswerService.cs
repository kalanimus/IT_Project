using Core.Entities;
using Core.Interfaces;

namespace Application.Services
{
    public class SurveyAnswerService : ISurveyAnswerService
    {
        private readonly ISurveyAnswerRepository _surveyAnswerRepository;

        public SurveyAnswerService(ISurveyAnswerRepository surveyAnswerRepository)
        {
            _surveyAnswerRepository = surveyAnswerRepository;
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
    }
}