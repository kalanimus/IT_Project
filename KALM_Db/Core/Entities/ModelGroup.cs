using System;

namespace Core.Entities;

public class ModelGroup
{
  public int Id { get; set; }
  public string Group_Name { get; set; }
  public List<ModelGroupStudent> GroupStudents { get; set; } = new List<ModelGroupStudent>();
  public List<ModelGroupTeacher> GroupTeachers { get; set; } = new List<ModelGroupTeacher>();
}
