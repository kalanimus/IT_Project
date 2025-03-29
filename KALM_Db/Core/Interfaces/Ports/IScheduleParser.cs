using Core.Entities;
namespace Core.Interfaces;

public interface IScheduleParser
{
  Task<List<ModelGroupTeacher>> ParseScheduleAsync(Stream fileStream);
}
