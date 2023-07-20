using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using Microsoft.Extensions.Configuration;
using Repositories.Abstractions;
using Repositories.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        
        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            
        }


        public async Task<IEnumerable<User>> GetAllNotDeletedUsers()
        {
            try
            {
                return await unitOfWork.UserRepository.GetAllNotDeletedUsers();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                return await unitOfWork.UserRepository.GetAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<User> GetUserById(int id)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
                return user!;
            else
                return user;
        }




        public async Task<string> AuthenticateAndReturnJwt(LogInVM data, JwtSettings jwt)
        {
            string msg = string.Empty;

            try
            {
                // authenticate user
                var result = await unitOfWork.UserRepository.Authenticate(data);

                if(result != string.Empty)
                   return msg = result;

                // fetch the authenticated user data 
                var userForToken = await unitOfWork.UserRepository
                        .FindUserByUserName(data.UserName);

                if (userForToken == null)
                    return msg += "Invalid User\n";

                // create jwt token
                string jwtToken = unitOfWork.UserRepository
                    .CreateJwtToken(userForToken, jwt);

                if (jwtToken == string.Empty)
                    return msg += "Jwt token error";

                //if (jwtToken == string.Empty)
                //    return msg += "Failed to create JWT token";

                // update the authenticated user data with jwt token
                userForToken.Token = jwtToken;
                unitOfWork.UserRepository.Update(userForToken);
                await unitOfWork.CompleteAsync();

                // returns jwt token
                if (jwtToken != null)
                    return jwtToken;
                else
                    return msg += "Failed to create JWT token";
            }
            catch (Exception)
            {
                throw;
            }
        }


        //public async Task<bool> Register(RegisterVM data)
        //{
        //    if (data == null)
        //        return false;

        //    try
        //    {
        //         var result = await unitOfWork.UserRepository.Register(data);

        //         if (result == string.Empty)
        //         {
        //             await unitOfWork.CompleteAsync();
        //             return true;
        //         }
        //         else
        //             return false;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public async Task<string> Register(RegisterVM data)
        {
            string msg = string.Empty;

            if (data == null)
                return msg = "Failed to register new user";

            try
            {
                var result = await unitOfWork.UserRepository.Register(data);

                if (result == string.Empty)
                {
                    await unitOfWork.CompleteAsync();
                    return msg;
                }
                else
                    return msg = result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> TempDelete(int id)
        {
            // check if user exists
            var dataToTempDelete = await unitOfWork.UserRepository
                                         .GetByIdAsync(id);
            if (dataToTempDelete == null)
                return false;

            // check if user is deleted
            var isUserDeleted = await unitOfWork.UserRepository
                                     .IsUserDeleted(dataToTempDelete.UserId);
            if (isUserDeleted)
                return false;

            // check if the user is a customer
            var isCustomer = await unitOfWork.UserRepository.IsCustomer(id);
            if (isCustomer)
                return false;

            try
            {
                var result = await unitOfWork.UserRepository.TempDeleteUser(dataToTempDelete);
                if (result)
                {
                    await unitOfWork.CompleteAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AssignRole(AssignRoleVM data)
        {
            // check if the user exists
            var userData = await unitOfWork.UserRepository.GetByIdAsync(data.UserId);

            if(userData == null) 
                return false;

            try
            {
                var result = await unitOfWork.UserRepository.AssignRole(data);
                if (result)
                {
                    await unitOfWork.CompleteAsync();
                    return true;
                }   
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
