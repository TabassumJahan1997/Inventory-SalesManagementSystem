using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<IEnumerable<User>> GetAllNotDeletedUsers();
        //Task<bool> Authenticate(LogInVM data);
        Task<string> Authenticate(LogInVM data);

        //Task<bool> Register(RegisterVM data);
        Task<string> Register(RegisterVM data);

        //bool CheckPasswordStrength(string password);
        string CreateJwtToken(User data, JwtSettings jwt);
        Task<User> FindUserByUserName(string userName);
        Task<bool> TempDeleteUser(User data);
        Task<bool> IsUserDeleted(int id);
        Task<bool> IsCustomer(int id);
        Task<bool> AssignRole(AssignRoleVM data);
    }
}
