namespace Application.DTOs;

// RegisterRequestDto.cs
public class VerifyRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int Code { get; set; }
}

public class VerifyResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string ?Message { get; set; }

}