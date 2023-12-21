using Mongo.Services.AuthAPI.Models;

namespace Mongo.Services.AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
