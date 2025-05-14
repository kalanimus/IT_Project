namespace Application.DTOs;
public class UserDto
{
    public int Id { get; set;}
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Balance { get; set; }
    public float Rating { get; set; }
    public int ActivityRate { get; set; }
    public string ?Email { get; set; }
    public int ?VerificationCode { get; set; }
    public bool IsConfirmed { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
}