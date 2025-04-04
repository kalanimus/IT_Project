using System;

namespace Core.Entities;

public class ModelGroupTeacher
{
  public int GroupId { get; set; }
  public int TeacherId { get; set; }
  public int SubjectId { get; set; }
  public ModelGroup Group { get; set;}
  public ModelUser Teacher { get; set; }
  public ModelSubject Subject { get; set; }
}
