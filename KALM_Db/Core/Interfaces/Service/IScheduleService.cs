namespace Core.Interfaces;
public interface IScheduleService
{
    Task UploadScheduleAsync(Stream fileStream);
}