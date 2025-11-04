using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.ProductBrand;
using ProductService.ApplicationContract.Interfaces.ProductBrand;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.ProductBrand;
using ProductService.InfrastructureContract.Interfaces.Query.Product;
using ProductService.InfrastructureContract.Interfaces.Query.ProductBrand;
using System.Net;

namespace ProductService.Application.Services.ProductBrand
{
    public class ProductBrandAppService : IProductBrandAppService
    {
        private readonly IProductBrandCommandRepository _productBrandCommandRepository;
        private readonly IProductBrandQueryRepository _productBrandQueryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductQueryRespository _productQueryRespository;

        public ProductBrandAppService(IProductBrandCommandRepository productBrandCommandRepository,
            IProductBrandQueryRepository productBrandQueryRepository, IMapper mapper, IUnitOfWork unitOfWork
            ,IProductQueryRespository productQueryRespository)
        {
            _productBrandCommandRepository = productBrandCommandRepository;
            _productBrandQueryRepository = productBrandQueryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productQueryRespository = productQueryRespository;
        }

        #region Create
        public async Task<BaseResponseDto<ProductBrandDto>> CreateProductBrand(ProductBrandDto productBrandDto)
        {
            var output = new BaseResponseDto<ProductBrandDto>
            {
                Message = "خطا در درج برند برند محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var brand = await _productBrandQueryRepository.GetQueryable().AnyAsync(b => b.Name == productBrandDto.Name);
            if (brand)
            {
                output.Message = "برند محصول از قبل ثبت شده است";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotAcceptable;
                return output;
            }
            var mapped = _mapper.Map<ProductBrandEntity>(productBrandDto);
            _productBrandCommandRepository.Add(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = $"برند {productBrandDto.Name} با موفقیت درج شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Edit
        public async Task<BaseResponseDto<ProductBrandDto>> EditProductBrand(int id, ProductBrandDto productBrandDto)
        {
            var output = new BaseResponseDto<ProductBrandDto>
            {
                Message = "خطا در به روز رسانی برند محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var brandExist = await _productBrandQueryRepository.GetQueryable().FirstOrDefaultAsync(b => b.Id == id);
            if (brandExist == null)
            {
                output.Message = "برند محصول یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var mapped = _mapper.Map(productBrandDto, brandExist);
            _productBrandCommandRepository.Edit(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "برند محصول با موفقیت به روزرسانی شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region GetAll
        public async Task<BaseResponseDto<List<ProductBrandDto>>> GetAllProductBrands()
        {
            var output = new BaseResponseDto<List<ProductBrandDto>>
            {
                Message = "خطا در بازیابی برند محصولات ها",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var details = await _productBrandQueryRepository.GetQueryable().Select(c => new ProductBrandDto
            {
                Name = c.Name,
                Description = c.Description
            }).ToListAsync();
            if (!details.Any())
            {
                output.Message = "برند محصولات  موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            output.Message = "برند محصولات  با موفقیت دریافت شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            output.Data = details;
            return output;
        }
        #endregion

        #region Get
        public async Task<BaseResponseDto<ProductBrandDto>> GetProductBrand(int id)
        {
            var output = new BaseResponseDto<ProductBrandDto>
            {
                Message = "خطا در بازیابی برند محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var brand = await _productBrandQueryRepository.GetQueryable()
                .Where(c => c.Id == id)
                .Select(c => new ProductBrandDto { Name = c.Name, Description = c.Description })
                .FirstOrDefaultAsync();
            if (brand == null)
            {
                output.Message = "برند محصول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }

            output.Message = "برند محصول با موفقیت دریافت شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            output.Data = brand;

            return output;
        }
        #endregion

        #region Delete
        public async Task<BaseResponseDto<ProductBrandDto>> DeleteProductBrand(int id)
        {
            var output = new BaseResponseDto<ProductBrandDto>
            {
                Message = "خطا در حذف برند ",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };

            var brandExist = await _productBrandQueryRepository.GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (brandExist == null)
            {
                output.Message = "برند محصول موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            if (await _productQueryRespository.GetQueryable().AnyAsync(p=> p.ProductBrandId == id))
            {
                output.Message = "این برند محصولات فعال داره و نمی‌تونه حذف بشه";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                return output;
            }
            _productBrandCommandRepository.Delete(brandExist);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "برند  با موفقیت حذف شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion
    }
}
