namespace Application.DTOs;

// AuthResponseDto.cs
public class AuthResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}