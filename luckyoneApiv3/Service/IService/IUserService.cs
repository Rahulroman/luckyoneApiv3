using luckyoneApiv3.Entity;
using Microsoft.AspNetCore.Mvc;

namespace luckyoneApiv3.Service.IService
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUserList();
        Task<Users> GetUserByID(int id);
    }
}
