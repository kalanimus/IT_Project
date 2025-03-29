using System;

namespace DatabaseModels;

public class ModelPermissionForRole
{
  public int PermissionId { get; set; }
  public ModelPermission Permission { get; set; }
  public int RoleId { get; set; }
  public ModelRole Role { get; set; }
}
