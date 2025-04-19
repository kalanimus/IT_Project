using Core.Interfaces;
using System.Text;

namespace Application.Services
{
    public class TransliterationService : ITransliterationService
    {
        public string ConvertToTranslit(string text)
        {
            var sb = new StringBuilder();

            foreach (char c in text)
            {
                string translitChar = c switch
                {
                    'а' => "a", 'б' => "b", 'в' => "v", 'г' => "g", 'д' => "d",
                    'е' => "e", 'ё' => "yo", 'ж' => "zh", 'з' => "z", 'и' => "i",
                    'й' => "y", 'к' => "k", 'л' => "l", 'м' => "m", 'н' => "n",
                    'о' => "o", 'п' => "p", 'р' => "r", 'с' => "s", 'т' => "t",
                    'у' => "u", 'ф' => "f", 'х' => "kh", 'ц' => "ts", 'ч' => "ch",
                    'ш' => "sh", 'щ' => "sch", 'ъ' => "", 'ы' => "y", 'ь' => "",
                    'э' => "e", 'ю' => "yu", 'я' => "ya",
                    'А' => "A", 'Б' => "B", 'В' => "V", 'Г' => "G", 'Д' => "D",
                    'Е' => "E", 'Ё' => "Yo", 'Ж' => "Zh", 'З' => "Z", 'И' => "I",
                    'Й' => "Y", 'К' => "K", 'Л' => "L", 'М' => "M", 'Н' => "N",
                    'О' => "O", 'П' => "P", 'Р' => "R", 'С' => "S", 'Т' => "T",
                    'У' => "U", 'Ф' => "F", 'Х' => "Kh", 'Ц' => "Ts", 'Ч' => "Ch",
                    'Ш' => "Sh", 'Щ' => "Sch", 'Ъ' => "", 'Ы' => "Y", 'Ь' => "",
                    'Э' => "E", 'Ю' => "Yu", 'Я' => "Ya", '1' => "1", '2' => "2",
                    '3' => "3", '4' => "4", '5' => "5", '6' => "6", '7' => "7",
                    '8' => "8", '9' => "9",  '0' => "0",
                    _ => c.ToString()
                };

                sb.Append(translitChar);
            }

            return sb.ToString();
        }
    }
}