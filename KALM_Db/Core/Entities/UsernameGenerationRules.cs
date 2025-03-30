namespace Core.Models;

public static class UsernameGenerationRules
{
    public const string Format = "{0}{1}_{2}"; // ФамилияИО_XXXX
    public const int RandomDigitsLength = 4;
}