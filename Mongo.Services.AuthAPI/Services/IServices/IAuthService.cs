using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Mongo.Services.AuthAPI.Models.Dto;

namespace Mongo.Services.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        Task<bool> AssignRole(string email, string roleName);
    }
}
