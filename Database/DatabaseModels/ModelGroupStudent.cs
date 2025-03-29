using System;

namespace DatabaseModels;

public class ModelGroupStudent
{
  public int GroupId { get; set; }
  public ModelGroup Group { get; set;}
  public int StudentId { get; set; }
  public ModelUser Student { get; set; }
}
