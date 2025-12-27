using luckyoneApiv3.Entity;
using luckyoneApiv3.Helper;
using Microsoft.AspNetCore.Mvc;
using static luckyoneApiv3.Models.UserModels;

namespace luckyoneApiv3.Service.IService
{
    public interface IUserService
    {
       Task<UserDto> GetUserById(int id);

        Task<UserDto> UpdateUserProfile(UpdateProfileRequest request, int userId);

        Task<PaginatedResponse<UserDto>> GetAllUsers(int page, int limit, string? search);


    }
}
