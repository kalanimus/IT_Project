namespace Core.Entities;

public class ModelUser
{
  public int Id { get; set; }
  public string FullName { get; set; }
  public string ?Username { get; set; }
  public string ?PasswordHash { get; set; }
  public int Balance { get; set; }
  public string ?Email { get; set; }
  public int ?VerificationCode { get; set; }
  public bool IsConfirmed { get; set; }
  public int RoleId { get; set; }
  public ModelRole Role { get; set; }
  public List<ModelGroupStudent> GroupStudents { get; set; } = new List<ModelGroupStudent>();
  public List<ModelGroupTeacher> GroupTeachers { get; set; } = new List<ModelGroupTeacher>();
}
