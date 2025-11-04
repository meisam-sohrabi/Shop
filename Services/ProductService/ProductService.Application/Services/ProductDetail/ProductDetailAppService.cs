using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.ProductDetail;
using ProductService.ApplicationContract.Interfaces.ProductDetail;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.ProductDetail;
using ProductService.InfrastructureContract.Interfaces.Query.Product;
using ProductService.InfrastructureContract.Interfaces.Query.ProductDetail;
using System.Net;

namespace ProductService.Application.Services.ProductDetail
{
    public class ProductDetailAppService : IProductDetailAppService
    {
        private readonly IProductDetailCommandRepository _productDetailCommandRepository;
        private readonly IProductDetailQueryRepository _productDetailQueryRepository;
        private readonly IProductQueryRespository _productQueryRespository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductDetailAppService(IProductDetailCommandRepository productDetailCommanRepository,
            IProductDetailQueryRepository productDetailQueryRepository,
            IProductQueryRespository productQueryRespository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productDetailCommandRepository = productDetailCommanRepository;
            _productDetailQueryRepository = productDetailQueryRepository;
            _productQueryRespository = productQueryRespository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #region Create
        public async Task<BaseResponseDto<ProductDetailResponseDto>> CreateProductDetail(ProductDetailRequestDto ProductDetailDto)
        {
            var output = new BaseResponseDto<ProductDetailResponseDto>
            {
                Message = "خطا در درج جزئیات محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var productExist = await _productQueryRespository.GetQueryable().AnyAsync(c => c.Id == ProductDetailDto.ProductId);
            if (!productExist)
            {
                output.Message = "محصول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var mapped = _mapper.Map<ProductDetailEntity>(ProductDetailDto);
            _productDetailCommandRepository.Add(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "جزئیات محصول با موفقیت درج شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion


        #region Edit
        public async Task<BaseResponseDto<ProductDetailResponseDto>> EditProductDetail(int id, ProductDetailRequestDto ProductDetailDto)
        {
            var output = new BaseResponseDto<ProductDetailResponseDto>
            {
                Message = "خطا در به روز رسانی جزئیات محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var detailExist = await _productDetailQueryRepository.GetQueryable().FirstOrDefaultAsync(b => b.Id == id);
            if (detailExist == null)
            {
                output.Message = "جزئیات محصول یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var mapped = _mapper.Map(ProductDetailDto, detailExist);
            _productDetailCommandRepository.Edit(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "جزئیات محصول با موفقیت به روزرسانی شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region GetAll
        public async Task<BaseResponseDto<List<ProductDetailResponseDto>>> GetAllProductDetails()
        {
            var output = new BaseResponseDto<List<ProductDetailResponseDto>>
            {
                Message = "خطا در بازیابی  جزئیات محصولات ",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var details = await _productDetailQueryRepository.GetQueryable().Select(c => new ProductDetailResponseDto
            {
                Size = c.Size,
                Color = c.Color,
                Description = c.Description
            }).ToListAsync();
            if (!details.Any())
            {
                output.Message = "جزئیات محصولات موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            output.Message = "جزیئات محصولات با موفقیت دریافت شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            output.Data = details;
            return output;
        }
        #endregion

        #region Get
        public async Task<BaseResponseDto<ProductDetailResponseDto>> GetProductDetail(int id)
        {
            var output = new BaseResponseDto<ProductDetailResponseDto>
            {
                Message = "خطا در بازیابی جزئیات محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var detail = await _productDetailQueryRepository.GetQueryable()
                .Where(c => c.Id == id)
                .Select(c => new ProductDetailResponseDto
                {
                    Size = c.Size,
                    Color = c.Color,
                    Description = c.Description
                })
                .FirstOrDefaultAsync();
            if (detail == null)
            {
                output.Message = "جزئیات محصول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }

            output.Message = "جزئیات با موفقیت دریافت شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            output.Data = detail;

            return output;
        }
        #endregion

        #region Delete
        public async Task<BaseResponseDto<ProductDetailResponseDto>> DeleteProductDetail(int id)
        {
            var output = new BaseResponseDto<ProductDetailResponseDto>
            {
                Message = "خطا در حذف جزئیات محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };

            var detailExist = await _productDetailQueryRepository.GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (detailExist == null)
            {
                output.Message = "جزئیات محصول موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }

            _productDetailCommandRepository.Delete(detailExist);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "جزئیات محصول با موفقیت حذف شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion
    }
}
