using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PricingService.ApplicationContract.DTO.Base;
using PricingService.ApplicationContract.DTO.ProductPrice;
using PricingService.ApplicationContract.Interfaces.ProductPrice;
using PricingService.Domain.Entities;
using PricingService.InfrastructureContract.Interfaces;
using PricingService.InfrastructureContract.Interfaces.Command.ProductPrice;
using PricingService.InfrastructureContract.Interfaces.Query.ProductPrice;
using System.Net;

namespace PricingService.Application.Services.ProductPrice
{
    public class ProductPriceAppService : IProductPriceAppService
    {
        private readonly IProductPriceCommandRepository _productPriceCommandRepository;
        private readonly IProductPriceQueryRepository _productPriceQueryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductPriceAppService(IProductPriceCommandRepository productPriceCommandRepository,
            IProductPriceQueryRepository productPriceQueryRepository
            , IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _productPriceCommandRepository = productPriceCommandRepository;
            _productPriceQueryRepository = productPriceQueryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        #region Create
        public async Task<BaseResponseDto<ProductPriceResponseDto>> CreateProductPrice(ProductPriceRequestDto productPriceRequestDto)
        {
            var output = new BaseResponseDto<ProductPriceResponseDto>
            {
                Message = "خطا در درج قیمت محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            //var productDetailExist = await _productDetailQueryRepository.GetQueryable().AnyAsync(c => c.Id == productPriceRequestDto.ProductDetailId);
            //if (!productDetailExist)
            //{
            //    output.Message = "محصول موردنظر وجود ندارد";
            //    output.Success = false;
            //    output.StatusCode = HttpStatusCode.NotFound;
            //    return output;
            //}
            var mapped = _mapper.Map<ProductPriceEntity>(productPriceRequestDto);
            await _productPriceCommandRepository.AddAsync(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "قیمت محصول با موفقیت درج شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Delete
        public async Task<BaseResponseDto<ProductPriceResponseDto>> DeleteProductPrice(int id)
        {
            var output = new BaseResponseDto<ProductPriceResponseDto>
            {
                Message = "خطا در حذف قیمت محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var priceExist = await _productPriceQueryRepository.GetQueryable().FirstOrDefaultAsync(c => c.Id == id);
            if (priceExist == null)
            {
                output.Message = "قیمت محصول یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            _productPriceCommandRepository.Delete(priceExist);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "قیمت محصول با موفقیت حذف شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Edit
        public async Task<BaseResponseDto<ProductPriceResponseDto>> EditProductPrice(ProductPriceRequestDto productPriceRequestDto)
        {
            var output = new BaseResponseDto<ProductPriceResponseDto>
            {
                Message = "خطا در به روز رسانی قیمت محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var priceExist = await _productPriceQueryRepository.GetQueryable().Where(c => c.ProductDetailId == productPriceRequestDto.ProductDetailId).OrderByDescending(c=> c.SetDate).FirstOrDefaultAsync();
            if (priceExist == null)
            {
                output.Message = "قیمت محصول یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var mapped = _mapper.Map(productPriceRequestDto, priceExist);
            _productPriceCommandRepository.Edit(priceExist);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "قیمت محصول با موفقیت به روزرسانی شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Get
        public async Task<BaseResponseDto<ProductPriceResponseDto>> GetProductPrice(int id)
        {
            var output = new BaseResponseDto<ProductPriceResponseDto>
            {
                Message = "خطا در بازیابی قیمت",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var price = await _productPriceQueryRepository.GetQueryable()
                        .Where(c => c.ProductDetailId == id)
                        .OrderByDescending(c => c.SetDate)
                        .Select(c => new ProductPriceResponseDto { Id = c.Id,Price = c.Price, SetDate = c.SetDate })
                        .FirstOrDefaultAsync();
            if (price == null)
            {
                output.Message = "قیمت محصول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            output.Message = "قیمت محصول موردنظر با موفقیت دریافت شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            output.Data = price;
            return output;
        }

        #endregion

        #region GetAll
        //public async Task<BaseResponseDto<List<ProductPriceResponseDto>>> GetAllProductPrice()
        //{
        //    var output = new BaseResponseDto<List<ProductPriceResponseDto>>
        //    {
        //        Message = "خطا در بازیابی  قیمت محصولات ",
        //        Success = false,
        //        StatusCode = HttpStatusCode.BadRequest
        //    };
        //    var prices = await _productPriceQueryRepository.GetQueryable().Select(c => new ProductPriceResponseDto
        //    {
        //        Price = c.Price,
        //        SetDate = c.SetDate
        //    }).ToListAsync();
        //    if (!prices.Any())
        //    {
        //        output.Message = "قیمت محصولات موردنظر وجود ندارد";
        //        output.Success = false;
        //        output.StatusCode = HttpStatusCode.NotFound;
        //        return output;
        //    }
        //    output.Message = "قیمت محصولات با موفقیت دریافت شد";
        //    output.Success = true;
        //    output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        //    output.Data = prices;
        //    return output;
        //}
        #endregion



    }
}
