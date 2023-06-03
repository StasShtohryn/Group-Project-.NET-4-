using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModel;

namespace ServerProject.Services
{
    internal interface IUserService
    {
        IEnumerable<User> GetUsers();
        Task UpdateUser(User user);
        Task AddUser(User user);
        Task DeleteUser(int userId);

    }
}
