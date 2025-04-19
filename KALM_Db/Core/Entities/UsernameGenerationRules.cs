namespace Core.Models;

public static class UsernameGenerationRules
{
    public const string FormatTeacher = "{0}{1}_{2}"; // ФамилияИО_XXXX
    public const string FormatStudent = "{0}{1}{2}"; // ФамилияИО_XXXX
    public const int RandomDigitsLength = 4;
}