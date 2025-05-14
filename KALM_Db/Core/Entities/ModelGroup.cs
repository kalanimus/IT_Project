using System;

namespace Core.Entities;

public class ModelGroup
{
  public int Id { get; set; }
  public string GroupName { get; set; }
  public List<ModelGroupStudent> GroupStudents { get; set; } = new List<ModelGroupStudent>();
  public List<ModelGroupTeacher> GroupTeachers { get; set; } = new List<ModelGroupTeacher>();
}
