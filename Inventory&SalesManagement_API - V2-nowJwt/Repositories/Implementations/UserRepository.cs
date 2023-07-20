using DatabaseModels.DatabaseContext;
using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Abstractions;
using Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(Inventory_Sales_DbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<User>> GetAllNotDeletedUsers()
        {
            try
            {
                var data = await _dbSet.Where(x=>x.isDeleted == false).ToListAsync();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }


        //public async Task<bool> Authenticate(LogInVM data)
        //{
        //    // get the user data 
        //    var userToLogin = await _dbSet.FirstOrDefaultAsync(x => x.UserName == data.UserName);

        //    // verify password
        //    bool isPassVerified = PasswordHasher.VerifyPassword(data.Password, userToLogin!.Password);

        //    if (data.UserName != null && isPassVerified == true)
        //        return true;
        //    else
        //        return false;
        //}


        public async Task<string> Authenticate(LogInVM data)
        {
            string msg = string.Empty;

            // get the user data 
            var userToLogin = await _dbSet.FirstOrDefaultAsync(x => x.UserName == data.UserName);

            if (userToLogin == null)
                return msg = "Invalid Username\n";

            // verify password
            bool isPassVerified = PasswordHasher.VerifyPassword(data.Password, userToLogin!.Password);

            if (!isPassVerified)
                return msg += "Invalid Password\n";

            return msg;

            //if (data.UserName != null && isPassVerified == true)
            //    return true;
            //else
            //    return false;
        }

        public string CreateJwtToken(User data, JwtSettings jwt)
         {

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

                new Claim(ClaimTypes.Role, data.Role!),
                new Claim(ClaimTypes.Name, $"{data.FirstName} {data.LastName}"),
                new Claim(ClaimTypes.Email, data.Email!)
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: jwt.Issuer,
                    audience: jwt.Audience,
                    claims: identity.Claims,
                    expires: DateTime.Now.AddSeconds((jwt.AccessTokenExpirationSeconds >0 || string.IsNullOrEmpty(jwt.AccessTokenExpirationSeconds.ToString()) ? jwt.AccessTokenExpirationSeconds : 3600)),
                    signingCredentials: credentials

                );

            if (token == null)
                return string.Empty;
            else
                return jwtTokenHandler.WriteToken(token);
        }

        //public async Task<bool> Register(RegisterVM data)
        //{
        //    if (data == null)
        //        return false;
        //    if (await IsEmailExists(data.Email!))
        //        return false;
        //    if (await IsUserNameExists(data.UserName))
        //        return false;
        //    if (!CheckPasswordStrength(data.Password))
        //        return false;

        //    var newUser = new User
        //    {
        //        FirstName = data.FirstName,
        //        LastName = data.LastName,
        //        Email = data.Email,
        //        UserName = data.UserName,
        //        Password = PasswordHasher.HashPassword(data.Password),
        //        Role = "User"
        //    };

        //    await _dbSet.AddAsync(newUser);

        //    return true;
        //}

        public async Task<string> Register(RegisterVM data)
        {
            string msg = string.Empty;

            if (data == null)
                return msg = "Invalid data";
            if (await IsEmailExists(data.Email!))
                return msg = "Email Exists";
            if (await IsUserNameExists(data.UserName))
                return msg = "User name Exists";

            msg = CheckPasswordStrength(data.Password);
            if (msg != string.Empty)
                return msg;

            var newUser = new User
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                UserName = data.UserName,
                Password = PasswordHasher.HashPassword(data.Password),
                Role = "User"
            };

            await _dbSet.AddAsync(newUser);

            return msg;
        }


        //------check if user name exists--------

        private async Task<bool> IsUserNameExists(string userName)
        {
            if (await _dbSet.AnyAsync(x => x.UserName == userName))
                return true;
            else
                return false;
        }

        //---------check if the email exists----------

        private async Task<bool> IsEmailExists(string email)
        {
            if (await _dbSet.AnyAsync(x => x.Email == email))
                return true;
            else
                return false;
        }


       

        private string CheckPasswordStrength(string password)
        {
            string msg = string.Empty;

            //password lenght check
            if (password.Length < 8)
                return msg = "Password must contain at least 8 characters";

            // lowercase letter check
            if (!(Regex.IsMatch(password, "[a-z]")))
                return msg = "Password must contain at least 1 lowercase letter";

            // uppercase letter check
            if (!(Regex.IsMatch(password, "[A-Z]")))
                return msg = "Password must contain at least 1 uppercase letter";

            // number check
            if (!(Regex.IsMatch(password, "[0-9]")))
                return msg = "Password must contain at least 1 numeric character";

            // special character check
            if (!(Regex.IsMatch(password, "[<,>,!,@,#,$,%,^,&,*,(,),\\[,\\],\\,/,.,',\",`,_,-]")))
                return msg = "Password must contain at least 1 special character";

            return msg;
        }

        public async Task<User> FindUserByUserName(string userName)
        {
            try
            {
                var user = await _dbSet.FirstOrDefaultAsync(x => x.UserName == userName);
                return user!;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> IsUserDeleted(int id)
        {
            var userData = await _dbSet.FirstOrDefaultAsync(x=>x.UserId == id);

            if(userData!.isDeleted == true) 
                return true;
            else
                return false;
        }


        public async Task<bool> TempDeleteUser(User data)
        {
            if(data == null)
                return false;

            try
            {
                data.isDeleted = true;
                _dbSet.Update(data);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AssignRole(AssignRoleVM data)
        {
            var userData = await _dbSet.FirstOrDefaultAsync(x=>x.UserId == data.UserId);

            if(userData == null)
                return false;

            try
            {
                userData.Role = data.Role;
                _dbSet.Update(userData);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsCustomer(int id)
        {
            var userOrderData = await _dbContext.tblOrder.FirstOrDefaultAsync(x=>x.CustomerId == id);

            if (userOrderData != null)
                return true;
            else
                return false;
        }
    }
}
