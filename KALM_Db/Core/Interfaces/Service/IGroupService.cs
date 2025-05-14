using Core.Entities;

namespace Core.Interfaces;

public interface IGroupService : IService<ModelGroup> {
  Task AddStudentToGroupAsync(int groupId, int studentId);
  Task AddTeacherToGroupAsync(ModelGroupTeacher groupTeacher);


}
