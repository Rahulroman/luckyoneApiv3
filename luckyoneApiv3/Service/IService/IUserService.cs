using luckyoneApiv3.Entity;
using Microsoft.AspNetCore.Mvc;
using static luckyoneApiv3.Models.UserModels;

namespace luckyoneApiv3.Service.IService
{
    public interface IUserService
    {
       Task<UserDto> GetUserById(int id);






    }
}
