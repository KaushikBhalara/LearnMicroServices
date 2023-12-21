using Mongo.Web.Models;
using Mongo.Web.Service.IService;
using Mongo.Web.Utility;

namespace Mongo.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            this._baseService = baseService;
             
        }
        public Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto?> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto>? GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product",
            });
        }

        public Task<ResponseDto?> GetProductAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}
