using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mongo.Services.ProductAPI.Data;
using Mongo.Services.ProductAPI.Models;
using Mongo.Services.ProductAPI.Models.Dto;

namespace Mongo.Services.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductAPIController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
            _response = new ResponseDto();

        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
