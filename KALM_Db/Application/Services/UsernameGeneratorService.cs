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

    public string Generate(string fullName)
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
                UsernameGenerationRules.Format,
                lastName,
                firstNameInitial,
                randomDigits
            );
    }
}
    