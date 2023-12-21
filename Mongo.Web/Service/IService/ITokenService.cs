namespace Mongo.Web.Service.IService
{
    public interface ITokenService
    {
        void SetToken(string token);
        string? GetToken();
        Task<bool> ClearToken();
    }
}
