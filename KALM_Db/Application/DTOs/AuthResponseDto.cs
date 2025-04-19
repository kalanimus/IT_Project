namespace Application.DTOs;

// AuthResponseDto.cs
public class AuthResponseDto
{
    public string ?Token { get; set; }
    public DateTime ?Expiration { get; set; }
    public string ?Message { get; set; }
    public bool ?RequireVerification { get; set; }
}