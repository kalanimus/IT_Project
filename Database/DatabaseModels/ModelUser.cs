namespace DatabaseModels;

public class ModelUser
{
  public int Id { get; set; }
  public string FullName { get; set; }
  public int RoleId { get; set; }
  public ModelRole Role { get; set; }
  public List<ModelGroupStudent> GroupStudents { get; set; } = new List<ModelGroupStudent>();
  public List<ModelGroupTeacher> GroupTeachers { get; set; } = new List<ModelGroupTeacher>();
}
