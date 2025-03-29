using System;

namespace Core.Entities;

public class ModelSubject
{
  public int Id { get; set; }
  public string SubjectName { get; set; }
  public List<ModelGroupTeacher> GroupTeachers { get; set; } = new List<ModelGroupTeacher>();
}
