using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mongo.Web.Models;
using Mongo.Web.Service.IService;
using Mongo.Web.Utility;
using Newtonsoft.Json;
using System.Text;
using static Mongo.Web.Utility.SD;

namespace Mongo.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;

        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
           HttpClient client=  _httpClientFactory.CreateClient("MongoApi");
            HttpRequestMessage message = new();
            message.RequestUri = new Uri(requestDto.Url);
            if(requestDto.Data!=null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data),Encoding.UTF8, "application/json");
            }

            if(withBearer)
            {
                var token = _tokenService.GetToken();
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme,token);
            }

            HttpResponseMessage? apiResponse = null;

            switch(requestDto.ApiType)
            {
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            apiResponse = await client.SendAsync(message);

            try
            {
                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };

                    case System.Net.HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Forbidden" };

                    case System.Net.HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };

                    case System.Net.HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };

                    default:
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(response);
                        return responseDto;
                }
            }
            catch (Exception ex)
            {
                return new() { IsSuccess = false, Message = ex.Message };
            }

        }
    }
}
