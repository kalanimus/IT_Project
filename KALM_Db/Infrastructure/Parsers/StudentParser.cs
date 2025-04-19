using Core.Interfaces;
using Core.Entities;
using CsvHelper;
using System.Globalization;

namespace Infrastructure.Parsers;

public class StudentsParser : IStudentsParser
{
  public async Task<List<ModelGroupStudent>> ParseStudentsAsync(Stream fileStream)
  {
    try
      {
        using var reader = new StreamReader(fileStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var items = new List<ModelGroupStudent>();
        await foreach (var record in csv.GetRecordsAsync<dynamic>())
        {
            // Создаем объект доменной модели
            items.Add(new ModelGroupStudent
            {
                // Показываем пример без ID (они будут установлены в сервисе)
                Group = new ModelGroup { Group_Name = record.Group },
                Student = new ModelUser 
                { 
                    FullName = record.FullName,
                    Username = record.CCNumber
                }
            });
        }
        return items;
      }
    catch (Exception ex)
      {
        throw new Exception(ex.ToString());
      }
  }
}
