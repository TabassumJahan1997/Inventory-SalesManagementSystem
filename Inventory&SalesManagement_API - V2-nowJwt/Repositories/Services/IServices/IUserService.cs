using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Services.IServices
{
    public interface IUserService
    {
        Task<string> AuthenticateAndReturnJwt(LogInVM data,JwtSettings jwt);
        //Task<bool> Register(RegisterVM data);
        Task<string> Register(RegisterVM data);

        Task<bool> AssignRole(AssignRoleVM data);
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> GetAllNotDeletedUsers();
        Task<User> GetUserById(int id);
        Task<bool> TempDelete(int id);

    }
}
