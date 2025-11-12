using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Dto;
using ProductService.Api.Helper;
//using ProductService.Application.Services.PermissionAttribute;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.Product;
using ProductService.ApplicationContract.DTO.Search;
using ProductService.ApplicationContract.DTO.Transaction;
using ProductService.ApplicationContract.Interfaces.Product;
using System.Net;
namespace ProductService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IValidator<ProductTransactionRequestDto> _transactionValidator;

        public ProductController(IProductAppService productAppService, IValidator<ProductTransactionRequestDto> transactionValidator)
        {
            _productAppService = productAppService;
            _transactionValidator = transactionValidator;
        }


        [HttpPost("ProductTransaction")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<ProductTransactionServiceDto>> ProductTransaction([FromForm] ProductTransactionRequestDto productTransactionDto)
        {
            var validation = _transactionValidator.Validate(productTransactionDto);
            var output = new BaseResponseDto<ProductTransactionServiceDto>();
            if (!validation.IsValid)
            {
                output.Message = "خطاهای اعتبارسنجی رخ داده است.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                output.ValidationErrors = validation.ToDictionary();
                return output;
            }
            string? url = null;
            if (productTransactionDto.File != null)
            {
                 url = await FileStorage.SaveFileAsync(productTransactionDto.File);
            }
            var transactionService = new ProductTransactionServiceDto
            {
                categoryId = productTransactionDto.categoryId,
                productBrandId = productTransactionDto.productBrandId,
                ProductName = productTransactionDto.ProductName,
                ProductDescription = productTransactionDto.ProductDescription,
                ProductQuantity = productTransactionDto.ProductQuantity,
                Size = productTransactionDto.Size,
                Color = productTransactionDto.Color,
                DetailDescription = productTransactionDto.DetailDescription,
                Price = productTransactionDto.Price,
                ImageUrl = url
            };
            return await _productAppService.ProductTransaction(transactionService);
        }

        //[HttpPost("Edit/{id}")]
        //[Authorize(Roles = "admin")]
        //public async Task<BaseResponseDto<ProductResponseDto>> Edit([FromRoute] int id, [FromBody] ProductRequestDto productDto)
        //{
        //    return await _productAppService.EditProduct(id, productDto);
        //}

        [HttpGet("GetAll")]
        public async Task<BaseResponseDto<List<ProductResponseDto>>> GetAll()
        {
            return await _productAppService.GetAllProduct();
        }

        [HttpGet("StoredProcedureGetAll")]
        public async Task<BaseResponseDto<List<ProductWithInventoryDto>>> StoreProcedureGetAll([FromQuery] string? search, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            return await _productAppService.GetProductWithInventory(search, start, end);
        }

        [HttpPost("StoredProcedureEditAToP")]
        public async Task<BaseResponseDto<ProductResponseDto>> StoreProcedureEditAToP([FromBody] ProductArabicToPersianDto productArabicToPersianDto)
        {
            return await _productAppService.EditArabicToPersianSP(productArabicToPersianDto);
        }

        [HttpGet("GetById/{id}")]
        public async Task<BaseResponseDto<ProductResponseDto>> GetById([FromRoute] int id)
        {
            return await _productAppService.GetProduct(id);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<ProductResponseDto>> Delete([FromRoute] int id)
        {
            return await _productAppService.DeleteProduct(id);
        }

        [HttpPost("Search")]
        public async Task<BaseResponseDto<List<SearchResponseDto>>> Search([FromBody] SearchRequestDto search)
        {
            return await _productAppService.AdvanceSearchProduct(search);
        }

    }
}
