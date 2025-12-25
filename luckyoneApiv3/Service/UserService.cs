using luckyoneApiv3.Data;
using luckyoneApiv3.Entity;
using luckyoneApiv3.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace luckyoneApiv3.Service
{
    public class UserService : IUserService
    {

        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Users>> GetAllUserList()
        {

            var result = await (from U in _context.User
                                select U
                               ).ToListAsync();

            if (result.Count == 0)
            {
                return null;
            }

            return result;
        }

        public async Task<Users> GetUserByID(int id)
        { 
            var User = await (from U in _context.User
                              where U.Id == id
                              select U
                             ).FirstOrDefaultAsync();


            if (User == null)
            {
                return null;
            }

            return User;

        }









    }
}
