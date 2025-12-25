using luckyoneApiv3.Entity;
using Microsoft.AspNetCore.Identity.Data;
using static luckyoneApiv3.Models.AuthModels;

namespace luckyoneApiv3.Service.IService
{
    public interface IAuthService
    {
        Task<ApiResponseResgisterDto> Register(RegisterRequestDto request);
        Task<ApiResponseLoginDto> loginDto(LoginRequestDto requestDto);

    }
}
