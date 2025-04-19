using Core.Interfaces;
using Core.Models;

namespace Application.Services;

public class UsernameGeneratorService : IUsernameGeneratorService
{
    private readonly ITransliterationService _transliterationService;
    private readonly Random _random;

    public UsernameGeneratorService(
        ITransliterationService transliterationService,
        Random random)
    {
        _transliterationService = transliterationService;
        _random = random;
    }

    public string GenerateTeacherUsername(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be empty");

            string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length < 2)
                throw new ArgumentException("Full name must include at leas 2 parametres");

            string lastName = _transliterationService.ConvertToTranslit(nameParts[0]);
            string firstNameInitial = _transliterationService.ConvertToTranslit(nameParts[1])[..1];
            
            if (nameParts.Length > 2){
              firstNameInitial += _transliterationService.ConvertToTranslit(nameParts[2])[..1];
            }

            string randomDigits = _random.Next(0, 10000).ToString("D4");

            return string.Format(
                UsernameGenerationRules.FormatTeacher,
                lastName,
                firstNameInitial,
                randomDigits
            );
    }

    public string GenerateStudentUsername(string fullName, string CCNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be empty");

            string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length < 2)
                throw new ArgumentException("Full name must include at leas 2 parametres");

            string lastNameInitial = _transliterationService.ConvertToTranslit(nameParts[0].ToLower())[..1];
            string firstNameInitial = _transliterationService.ConvertToTranslit(nameParts[1].ToLower())[..1];
            
            if (nameParts.Length > 2){
              firstNameInitial += _transliterationService.ConvertToTranslit(nameParts[2].ToLower())[..1];
            }

            string ccnumber = _transliterationService.ConvertToTranslit(CCNumber);

            return string.Format(
                UsernameGenerationRules.FormatStudent,
                lastNameInitial,
                firstNameInitial,
                ccnumber
            );
    }
}
    