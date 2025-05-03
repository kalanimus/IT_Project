using Core.Entities;
using Core.Interfaces;

namespace Application.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;

        public SurveyService(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        public async Task<ModelSurvey> GetByIdAsync(int id)
        {
            return await _surveyRepository.GetByIdAsync(id);
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