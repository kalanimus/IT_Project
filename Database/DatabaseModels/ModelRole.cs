namespace DatabaseModels;

public class ModelRole
{
  public int Id { get; set; }
  public string RoleName { get; set; }
  public List<ModelPermissionForRole> PermissionsForRoles { get; set; } = new List<ModelPermissionForRole>();
  public List<ModelUser> Users { get; set; } = new List<ModelUser>();
}
