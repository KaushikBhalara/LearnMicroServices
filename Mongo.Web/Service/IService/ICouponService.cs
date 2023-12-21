using Mongo.Web.Models;

namespace Mongo.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string couponCode);
        Task<ResponseDto>? GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto);



    }
}
