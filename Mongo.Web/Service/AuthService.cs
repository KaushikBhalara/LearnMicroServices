using Mongo.Web.Models;
using Mongo.Web.Service.IService;
using Mongo.Web.Utility;
using Newtonsoft.Json;

namespace Mongo.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        private readonly ITokenService tokenService;
        public AuthService(IBaseService baseService,ITokenService tokenService)
        {
            this._baseService = baseService;
            this.tokenService = tokenService;
        }
    
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole",
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var response =  await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/login",
            });

            if(response!=null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                //set token
                tokenService.SetToken(loginResponseDto.Token);

            }
            return response;
        }

        public async Task<bool> LogoutAsync()
        {
            return await tokenService.ClearToken();
        }


        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/register",
            });
        }
    }
}
