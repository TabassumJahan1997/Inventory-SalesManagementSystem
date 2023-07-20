using DatabaseModels.DatabaseContext;
using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Abstractions;
using Repositories.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventory_SalesManagement_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserService service;

        public UserController( IConfiguration configuration, IUserService service)
        {
            this.configuration = configuration;
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllNotDeletedUsers()
        {
            try
            {
                var data = await service.GetAllNotDeletedUsers();

                if (data != null)
                    return Ok(data);
                else
                    return NotFound();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users =  await service.GetAllUsers();

                if(users != null)
                    return Ok(users);
                else
                    return NotFound(new {Message = "Users Not Found"});
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LogInVM data)
        {

            if (data == null)
                return BadRequest(new
                {
                    Message = "Invalid Username and Password"
                });

            // gets the values from appsettings.json file
            var jwt = configuration.GetSection("JwtSettings")
                .Get<JwtSettings>();

            if (jwt == null)
                return BadRequest(new
                {
                    Message = "Invalid configuration data"
                });
            

            try
            {
                // authenticate user and gets the jwt token
                string jwtToken = await service
                    .AuthenticateAndReturnJwt(data,jwt);

                if (jwtToken.Length <= 50)
                    return BadRequest(jwtToken);

                if (jwtToken.Length > 50 && jwtToken != string.Empty)
                {
                    return Ok(new
                    {
                        Token = jwtToken,
                        Message = "Login Success"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Login Failed"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterVM data)
        {
            if (data == null)
                return BadRequest(new {Message = "Please , Provide appropriate data to register"});

            try
            {
                var result = await service.Register(data);

                if (result == string.Empty)
                {
                    return Ok(new
                    {
                        Message = "Registered Successfully"
                    });
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> TempDeleteUser(int id)
        {
            try
            {
                var result = await service.TempDelete(id);

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception)
            {
                throw;
            }

        }



        [HttpPut]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AssignRole(AssignRoleVM data)
        {
            if(data == null || !ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = await service.AssignRole(data);

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
