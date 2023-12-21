using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Mongo.Web.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Mongo.Web.Service
{
    public class TokenService : ITokenService
    {
        private readonly HttpContext _httpContext;
        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContext = httpContextAccessor.HttpContext;
            
        }
        public async Task<bool> ClearToken()
        {
            this._httpContext.Response.Cookies.Delete("jwtToken");
            await _httpContext.SignOutAsync();
            return true;
        }

        public string? GetToken()
        {
            return this._httpContext.Request.Cookies.FirstOrDefault(x=>x.Key=="jwtToken").Value;
            
        }

        public async void SetToken(string token)
        {
            this._httpContext.Response.Cookies.Append("jwtToken", token);
            await SignInAsync(token);
        }

        public async Task SignInAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);
            var claims = jwt.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }


    }
}
