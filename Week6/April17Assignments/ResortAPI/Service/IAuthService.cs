using OnlineResortBooking.DTOs.Auth;

namespace OnlineResortBooking.Service
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<bool> RegisterAsync(RegisterDto dto);
    }
}