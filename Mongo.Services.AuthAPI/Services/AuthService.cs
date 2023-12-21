using Microsoft.AspNetCore.Identity;
using Mongo.Services.AuthAPI.Data;
using Mongo.Services.AuthAPI.Models;
using Mongo.Services.AuthAPI.Models.Dto;
using Mongo.Services.AuthAPI.Services.IServices;

namespace Mongo.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(RoleManager<IdentityRole> roleManager,
                            UserManager<ApplicationUser> userManager,
                            AppDbContext appDbContext,
                            IJwtTokenGenerator jwtTokenGenerator
        )
        {
            _db = appDbContext;
            _jwtTokenGenerator  = jwtTokenGenerator;
            _roleManager = roleManager;
            _userManager = userManager;

        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user !=null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();


                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            var roles  = await _userManager.GetRolesAsync(user);
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (isValid)
            {

                //generate JWT token
                var token = _jwtTokenGenerator.GenerateToken(user,roles);
                return new()
                {
                    User = new UserDto()
                    {
                        Email = user.Email,
                        ID = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber
                    },
                    Token= token,
                };
            }
            return new() { User = null, Token = "" };
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToResturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault()?.Description;
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Error Encountered";

        }
    }
}
