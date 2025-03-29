using System;

namespace DatabaseModels;

public class ModelPermission
{
 public int Id { get; set; }
 public string PermissionName { get; set; }
 public List<ModelPermissionForRole> PermissionsForRoles { get; set; } = new List<ModelPermissionForRole>();
}
