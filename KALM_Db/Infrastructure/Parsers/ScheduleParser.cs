using Core.Interfaces;
using Core.Entities;
using CsvHelper;
using System.Globalization;

namespace Infrastructure.Parsers;

public class ScheduleParser : IScheduleParser
{
  public async Task<List<ModelGroupTeacher>> ParseScheduleAsync(Stream fileStream)
  {
    try
      {
        using var reader = new StreamReader(fileStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var items = new List<ModelGroupTeacher>();
        await foreach (var record in csv.GetRecordsAsync<dynamic>())
        {
            // Создаем объект доменной модели
            items.Add(new ModelGroupTeacher
            {
                // Показываем пример без ID (они будут установлены в сервисе)
                Group = new ModelGroup { Group_Name = record.Group },
                Teacher = new ModelUser 
                { 
                    FullName = record.Teacher,
                },
                Subject = new ModelSubject
                {
                    SubjectName = record.Subject
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
