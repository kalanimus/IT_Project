using Core.Entities;

namespace Core.Interfaces;

public interface IGroupTeacherService
{
  Task<ModelGroupTeacher> GetByDetailsAsync(string groupName, string subjectName, string teacherName);
 }
