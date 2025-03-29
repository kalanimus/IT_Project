using System;

namespace Core.Entities;

public class ModelGroupTeacher
{
  public ModelGroup Group { get; set;}
  public ModelUser Teacher { get; set; }
  public ModelSubject Subject { get; set; }
}
