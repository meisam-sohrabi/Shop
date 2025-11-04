using AutoMapper;
using Azure;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.ApplicationContract;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.Inventory;
using ProductService.ApplicationContract.DTO.Price;
using ProductService.ApplicationContract.DTO.Product;
using ProductService.ApplicationContract.DTO.Search;
using ProductService.ApplicationContract.DTO.Transaction;
using ProductService.ApplicationContract.Interfaces.Product;
using ProductService.ApplicationContract.Interfaces.RabbitMq.Inventory;
using ProductService.ApplicationContract.Interfaces.RabbitMq.Price;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Product;
using ProductService.InfrastructureContract.Interfaces.Command.ProductDetail;
using ProductService.InfrastructureContract.Interfaces.Query.Category;
using ProductService.InfrastructureContract.Interfaces.Query.Product;
using ProductService.InfrastructureContract.Interfaces.Query.ProductBrand;
using ProductService.InfrastructureContract.Interfaces.Query.ProductDetail;
using RedisService;
using System.Net;
namespace ProductService.Application.Services.Product
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductQueryRespository _productQueryRespository;
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductBrandQueryRepository _productBrandQueryRepository;
        private readonly IProductDetailQueryRepository _productDetailQueryRepository;
        private readonly IProductDetailCommandRepository _productDetailCommandRepository;
        private readonly ICacheAdapter _cacheAdapter;
        private readonly ILogger<ProductAppService> _logger;
        private readonly IValidator<ProductRequestDto> _productValidator;
        private readonly IValidator<ProductTransactionDto> _productTransactionValidator;
        private readonly IValidator<ProductArabicToPersianDto> _productArabicToPersianValidator;
        private readonly IRabbitPricePublisherAppService _rabbitPublishPrice;
        private readonly IRabbitInventoryPublisherAppService _rabbitPublishInventory;
        private readonly IUserAppService _userService;

        public ProductAppService(IProductQueryRespository productQueryRespository,
            IProductCommandRepository productCommandRepository,
            ICategoryQueryRepository categoryQueryRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IProductBrandQueryRepository productBrandQueryRepository
            , IProductDetailQueryRepository productDetailQueryRepository
            , IProductDetailCommandRepository productDetailCommandRepository
            , ICacheAdapter cacheAdapter
            , ILogger<ProductAppService> logger
            , IValidator<ProductRequestDto> Productvalidator
            , IValidator<ProductTransactionDto> productTransactionValidator
            ,IValidator<ProductArabicToPersianDto> productArabicToPersianValidator
            ,IRabbitPricePublisherAppService rabbitPublishPrice
            ,IRabbitInventoryPublisherAppService rabbitPublishInventory
            ,IUserAppService userService)
        {
            _productQueryRespository = productQueryRespository;
            _productCommandRepository = productCommandRepository;
            _categoryQueryRepository = categoryQueryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productBrandQueryRepository = productBrandQueryRepository;
            _productDetailQueryRepository = productDetailQueryRepository;
            _productDetailCommandRepository = productDetailCommandRepository;
            _cacheAdapter = cacheAdapter;
            _logger = logger;
            _productValidator = Productvalidator;
            _productTransactionValidator = productTransactionValidator;
            _productArabicToPersianValidator = productArabicToPersianValidator;
            _rabbitPublishPrice = rabbitPublishPrice;
            _rabbitPublishInventory = rabbitPublishInventory;
            _userService = userService;
        }


        #region Create
        //public async Task<BaseResponseDto<ProductResponseDto>> CreateProduct(ProductRequestDto productDto)
        //{
        //    var output = new BaseResponseDto<ProductResponseDto>
        //    {
        //        Message = "خطا در ایجاد محصول",
        //        Success = false,
        //        StatusCode = HttpStatusCode.BadRequest
        //    };
        //    var categoryExist = await _categoryQueryRepository.GetQueryable()
        //        .AnyAsync(c => c.Id == productDto.CategoryId);

        //    if (!categoryExist)
        //    {
        //        output.Message = "دسته‌بندی موردنظر وجود ندارد";
        //        output.Success = false;
        //        output.StatusCode = HttpStatusCode.NotFound;
        //        return output;
        //    }
        //    var brandExist = await _productBrandQueryRepository.GetQueryable()
        //        .AnyAsync(c => c.Id == productDto.ProductBrandId);
        //    if (!brandExist)
        //    {
        //        output.Message = "برند موردنظر وجود ندارد";
        //        output.Success = false;
        //        output.StatusCode = HttpStatusCode.NotFound;
        //        return output;
        //    }
        //    var mapped = _mapper.Map<ProductEntity>(productDto);
        //    _productCommandRepository.Add(mapped);
        //    var affectedRows = await _unitOfWork.SaveChangesAsync();
        //    if (affectedRows > 0)
        //    {
        //        output.Message = $"محصول {productDto.Name} با موفقیت ایجاد شد";
        //        output.Success = true;
        //    }
        //    output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
        //    return output;
        //}
        #endregion

        #region Edit
        public async Task<BaseResponseDto<ProductResponseDto>> EditProduct(int id, ProductRequestDto productDto)
        {
            var output = new BaseResponseDto<ProductResponseDto>
            {
                Message = "خطا در بروزرسانی محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            // در صورتی که بخوایم دسته بندی و برند و هم عوض کنیم اونوقت باید ووردی هم بدیم
            //var categoryExist = await _categoryQueryRepository.GetQueryable()
            //.AnyAsync(c => c.Id == productDto.CategoryId);

            //if (!categoryExist)
            //{
            //    output.Message = "دسته‌بندی موردنظر وجود ندارد";
            //    output.Success = false;
            //    output.StatusCode = HttpStatusCode.NotFound;
            //    return output;
            //}
            //var brandExist = await _productBrandQueryRepository.GetQueryable()
            //.AnyAsync(c => c.Id == productDto.ProductBrandId);
            //if (!brandExist)
            //{
            //    output.Message = "برند موردنظر وجود ندارد";
            //    output.Success = false;
            //    output.StatusCode = HttpStatusCode.NotFound;
            //    return output;
            //}


            // در این قسمت ولیدیشن صورت میگیره

            var validationResult = await _productValidator.ValidateAsync(productDto);

            // در این قسمت چک میشه و یک دیکشنری که کلید اسم پراپرتی هستش و ولیو لیستی از خطا

            if (!validationResult.IsValid)
            {
                output.Message = "خطاهای اعتبارسنجی رخ داده است.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                output.ValidationErrors = validationResult.ToDictionary();
                return output;
            }
            var productExist = await _productQueryRespository.GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (productExist == null)
            {
                output.Message = "محصول موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var oldQuantity = productExist.Quantity;
            var newQuantity = productDto.Quantity;
            var quantityChanged = oldQuantity != newQuantity;
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var mapped = _mapper.Map(productDto, productExist);
                _productCommandRepository.Edit(mapped);


                // use rabbit soon.
                //if (quantityChanged)  
                //{
                //    var diff = newQuantity - oldQuantity;
                //    var inventory = new ProductInventoryEntity
                //    {
                //        QuantityChange = diff,
                //        ProductId = productExist.Id,
                //    };
                //    await _productInventoryCommandRepository.Add(inventory);
                //}


                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                output.Message = "محصول  با موفقیت به روزرسانی شد";
                output.Success = true;
                output.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {

                await _unitOfWork.RollBackTransactionAsync();

                output.Message = "خطای غیرمنتظره رخ داد" + ex.Message;
                output.Success = false;
                output.StatusCode = HttpStatusCode.InternalServerError;
            }
            return output;
        }
        #endregion

        #region Delete
        public async Task<BaseResponseDto<ProductResponseDto>> DeleteProduct(int id)
        {
            var output = new BaseResponseDto<ProductResponseDto>
            {
                Message = "خطا در حذف محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };

            var productExist = await _productQueryRespository.GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (productExist == null)
            {
                output.Message = "محصول موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }

            _productCommandRepository.Delete(productExist);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "محصول با موفقیت حذف شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region GetAll
        public async Task<BaseResponseDto<List<ProductResponseDto>>> GetAllProduct()
        {
            var output = new BaseResponseDto<List<ProductResponseDto>>
            {
                Message = "خطا در بازیابی محصولات",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            List<ProductResponseDto> cachedData = null;

            try
            {
                cachedData = _cacheAdapter.Get<List<ProductResponseDto>>("Products");

            }
            catch (Exception ex)
            {
                _logger.LogWarning("Redis is not available. Falling back to database.");
            }

            if (cachedData != null && cachedData.Any())
            {
                output.Message = "محصولات با موفقیت از کش دریافت شد";
                output.Success = true;
                output.Data = cachedData;
                output.StatusCode = HttpStatusCode.OK;
                return output;
            }
            var products = await _productQueryRespository.GetQueryable()
                .Select(c => new ProductResponseDto { Name = c.Name, Description = c.Description, Quantity = c.Quantity })
                .ToListAsync();
            if (products.Any())
            {
                output.Message = "محصولات با موفقیت از دیتابیس دریافت شد";
                output.Success = true;
                output.Data = products;

                try
                {
                    _cacheAdapter.Set("Products", products);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to save products to Redis cache.");
                }


            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Get
        public async Task<BaseResponseDto<ProductResponseDto>> GetProduct(int id)
        {
            var output = new BaseResponseDto<ProductResponseDto>
            {
                Message = "خطا در بازیابی محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var product = await _productQueryRespository.GetQueryable()
                .Where(c => c.Id == id)
                .Select(c => new ProductResponseDto { Name = c.Name, Description = c.Description, Quantity = c.Quantity })
                .FirstOrDefaultAsync();
            if (product != null)
            {
                output.Message = "محصول با موفقیت دریافت شد";
                output.Success = true;
                output.Data = product;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Search
        public async Task<BaseResponseDto<List<SearchResponseDto>>> AdvanceSearchProduct(SearchRequestDto searchRequstDto)
        {
            var output = new BaseResponseDto<List<SearchResponseDto>>
            {
                Message = "خطا در دریافت محصول مورد نظر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };

            if (!string.IsNullOrEmpty(searchRequstDto.Search))
            {
                var searchProduct = await _productQueryRespository.GetQueryable()
                .Where(c => c.Name.Contains(searchRequstDto.Search) || c.Description.Contains(searchRequstDto.Search))
                .Join(_categoryQueryRepository.GetQueryable()
                .Where(c => c.IsActive == true),
                p => p.CategoryId, c => c.Id, (p, c) => new { product = p, category = c })
                .Join(_productBrandQueryRepository.GetQueryable(),
                pc => pc.product.ProductBrandId, b => b.Id, (pc, b) => new { pc.product, pc.category, brand = b })
                .Join(_productDetailQueryRepository.GetQueryable(),
                pcb => pcb.product.Id, d => d.ProductId, (pcb, d) => new { pcb.product, pcb.category, pcb.brand, detail = d })
                .Select(c => new SearchResponseDto
                {
                    produtName = c.product.Name,
                    productBrand = c.brand.Name,
                    categoryName = c.category.Name,
                    //Price = _productPriceQueryRepository.GetQueryable().Where(pr => pr.ProductDetailId == c.detail.Id).
                    //OrderByDescending(c => c.SetDate).Select(c => c.Price).FirstOrDefault(),
                    productColor = c.detail.Color,
                    productSize = c.detail.Size,
                }).ToListAsync();
                if (searchProduct.Any())
                {
                    output.Message = "محصول مورد نظر با موفقیت دریافت شد";
                    output.Success = true;
                    output.Data = searchProduct;
                }
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }

        #endregion

        #region ProductInventoryStoredProcedure

        public async Task<BaseResponseDto<List<ProductWithInventoryDto>>> GetProductWithInventory(string? search, DateTime? start, DateTime? end)
        {
            var output = new BaseResponseDto<List<ProductWithInventoryDto>>
            {
                Message = "خطا در دریافت محصول مورد نظر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var spData = await _productQueryRespository.GetProductsByDateAndTextAsync(search, start, end);
            if (spData.Any())
            {
                output.Message = "اطلاعات مورد نظر با موفقیت دریافت شد";
                output.Success = true;
                output.Data = spData;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Transaction
        public async Task<BaseResponseDto<ProductTransactionDto>> ProductTransaction(ProductTransactionDto productTransactionDto, int categoryId, int productBrandId)
        {
            var output = new BaseResponseDto<ProductTransactionDto>
            {
                Message = "خطا در درج اطلاعات",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };

            // در این قسمت ولیدیشن صورت میگیره
            var validationResult = await _productTransactionValidator.ValidateAsync(productTransactionDto);

            // در این قسمت چک میشه و یک دیکشنری که کلید اسم پراپرتی هستش و ولیو لیستی از خطا
            if (!validationResult.IsValid)
            {
                output.Message = "خطاهای اعتبارسنجی رخ داده است.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                output.ValidationErrors = validationResult.ToDictionary();
                return output;
            }
            try
            {
                var categoryExist = await _categoryQueryRepository.GetQueryable()
                    .FirstOrDefaultAsync(c => c.Id == categoryId);
                if (categoryExist == null)
                {
                    output.Message = "دسته بندی یافت نشد";
                    output.Success = false;
                    output.StatusCode = HttpStatusCode.NotFound;
                    return output;
                }
                var brandExist = await _productBrandQueryRepository.GetQueryable()
                    .FirstOrDefaultAsync(b => b.Id == productBrandId);
                if (brandExist == null)
                {
                    output.Message = "برند یافت نشد";
                    output.Success = false;
                    output.StatusCode = HttpStatusCode.NotFound;
                    return output;
                }
                await _unitOfWork.BeginTransactionAsync();


                var product = _mapper.Map<ProductEntity>(productTransactionDto.Product);
                product.CategoryId = categoryExist.Id;
                product.ProductBrandId = brandExist.Id;
                product.CreateBy = _userService.GetCurrentUser();
                _productCommandRepository.Add(product);
                await _unitOfWork.SaveChangesAsync();

                var detail = _mapper.Map<ProductDetailEntity>(productTransactionDto.ProductDetail);
                detail.ProductId = product.Id;
                detail.CreateBy = _userService.GetCurrentUser();
                _productDetailCommandRepository.Add(detail);
                await _unitOfWork.SaveChangesAsync();

                if (productTransactionDto.ProductImage.ImageUrl != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(productTransactionDto.ProductImage.ImageUrl.FileName);
                    if (productTransactionDto.ProductImage.ImageUrl.Length > 1048576)
                    {
                        throw new Exception("حجم فایل تصویر بیشتر از ۱ مگابایت است");

                    }
                    if (!allowedExtensions.Contains(fileExtension.ToLower()))
                    {
                        throw new Exception("نوع فایل نامعتبر است");
                    }

                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "userimage");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var url = Path.Combine(uploadFolder, fileName);
                    using (var fileStream = new FileStream(url, FileMode.Create))
                    {
                        await productTransactionDto.ProductImage.ImageUrl.CopyToAsync(fileStream);
                    }

                    var image = _mapper.Map<ProductImageEntity>(productTransactionDto.ProductImage);
                    image.ProductDetailId = detail.Id;
                    image.CreateBy = _userService.GetCurrentUser();
                }

               var sentPrice = await _rabbitPublishPrice.PublishMessage(new PriceToPublishDto
                {
                    ProductDetailId = detail.Id,
                    Price = productTransactionDto.Price,
                });

                if (!sentPrice)
                {
                    throw new Exception("خطا در ثبت قیمت");
                }

                var sentInventory = await _rabbitPublishInventory.PublishMessage(new InventoryAddToPublishDto
                {
                    QuantityChange = +product.Quantity,
                    ProductId = product.Id,
                });

                if (!sentInventory)
                {
                    throw new Exception("خطا در ثبت انبار");
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                output.Message = "محصول  با موفقیت ایجاد شد";
                output.Success = true;
                output.StatusCode = HttpStatusCode.Created;

            }
            catch (Exception ex)
            {

                await _unitOfWork.RollBackTransactionAsync();

                output.Message = "خطای غیرمنتظره رخ داد" + ex.Message;
                output.Success = false;
                output.StatusCode = HttpStatusCode.InternalServerError;
            }
            return output;
        }

        #endregion

        #region ProductsReport
        public async Task<List<ProductResponseDto>> GetProductsReport()
        {
            var products = await _productQueryRespository.GetQueryable()
                .Select(c => new ProductResponseDto { Name = c.Name, Description = c.Description, Quantity = c.Quantity })
                .ToListAsync();
            if (products.Any())
            {
                return products;
            }

            return new List<ProductResponseDto>();

        }
        #endregion

        #region EditAToPUsingSP
        public async Task<BaseResponseDto<ProductResponseDto>> EditArabicToPersianSP(ProductArabicToPersianDto productArabicToPersianDto)
        {
            var output = new BaseResponseDto<ProductResponseDto>
            {
                Message = "خطا در بروزرسانی محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var validationResult = await _productArabicToPersianValidator.ValidateAsync(productArabicToPersianDto);
            if (!validationResult.IsValid)
            {
                output.Message = "خطاهای اعتبارسنجی رخ داده است.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                output.ValidationErrors = validationResult.ToDictionary();
                return output;
            }
            await _productCommandRepository.EditArabicToPersianSP(productArabicToPersianDto.Start, productArabicToPersianDto.End);
            output.Message = "محصول  با موفقیت به روزرسانی شد";
            output.Success = true;
            output.StatusCode = HttpStatusCode.OK;
            return output;
        }
    }
    #endregion
}






#region Different ways of search query
/*var searchProduct = await _productQueryRespository.GetQueryable()
                .Where(c => c.Name.Contains(searchRequstDto.Search) || c.Description.Contains(searchRequstDto.Search))
                .Include(c => c.Category)
                .Include(c => c.ProductBrand)
                .Include(c => c.ProductDetails)
                .Select(c => new SearchResponseDto
                {
                    categoryName = c.Category.Name,
                    productBrand = c.ProductBrand.Name,
                    produtName = c.Name,
                    productDetail = c.ProductDetails.Select(c => new ProductDetailResponseDto { Description = c.Description, Size = c.Size, Price = c.Price }).ToList()
                }).ToListAsync();*/




/*from prodcut in _productQueryRespository.GetQueryable()
                                join category in _categoryQueryRepository.GetQueryable()
                                on prodcut.CategoryId equals category.Id
                                where category.IsActive == true
                                join brand in _productBrandQueryRepository.GetQueryable()
                                on prodcut.ProductBrandId equals brand.Id
                                join detail in _productDetailQueryRepository.GetQueryable()
                                on prodcut.Id equals detail.ProductId
                                select new SearchResponseDto
                                {
                                   produtName = prodcut.Name,
                                   productBrand = brand.Name,
                                   categoryName = category.Name,
                                   productDetail = detail.Price
                                };*/

/* var searchProduct = await _productQueryRespository.GetQueryable()
                .Where(c => c.Name.Contains(searchRequstDto.Search) || c.Description.Contains(searchRequstDto.Search))
                .Join(_categoryQueryRepository.GetQueryable().Where(c => c.IsActive == true),
                p => p.CategoryId, c => c.Id, (p, c) => new { product = p, category = c })
                .Join(_productBrandQueryRepository.GetQueryable(),
                pc => pc.product.ProductBrandId, b => b.Id, (pc, b) => new { pc.product, pc.category, brand = b })
                .Select(s => new SearchResponseDto
                {
                    produtName = s.product.Name,
                    productBrand = s.brand.Name,
                    categoryName = s.category.Name,
                    productDetail = s.product.ProductDetails
                    .Select(c => new ProductDetailResponseDto { Description = c.Description, Size = c.Size, Price = c.Price }).ToList(),
                }).ToListAsync();*/


//Select(s => new SearchResponseDto
//{
//    produtName = s.product.Name,
//    productBrand = s.brand.Name,
//    categoryName = s.category.Name,
//    productDetail = s.product.ProductDetails
//                    .Select(c => new ProductDetailResponseDto { Description = c.Description, Size = c.Size, Price = c.Price }).ToList(),
//}


// 2
/*var searchProduct = await _productQueryRespository.GetQueryable()
                .Where(c => c.Name.Contains(searchRequstDto.Search) || c.Description.Contains(searchRequstDto.Search))
                .Join(_categoryQueryRepository.GetQueryable().Where(c => c.IsActive == true),
                p => p.CategoryId, c => c.Id, (p, c) => new { product = p, category = c })
                .Join(_productBrandQueryRepository.GetQueryable(),
                pc => pc.product.ProductBrandId, b => b.Id, (pc, b) => new { pc.product, pc.category, brand = b })
                .SelectMany(c=> c.product.ProductDetails.DefaultIfEmpty(),(pcb,d)=> new SearchResponseDto
                {
                    produtName = pcb.product.Name,
                    productBrand = pcb.brand.Name,
                    categoryName = pcb.category.Name,
                    productDetail = d.Price
                }).ToListAsync();*/ // correct one , using left join


// 1 
/* var searchProduct = await _productQueryRespository.GetQueryable()
                .Where(c => c.Name.Contains(searchRequstDto.Search) || c.Description.Contains(searchRequstDto.Search))
                .Join(_categoryQueryRepository.GetQueryable().Where(c => c.IsActive == true),
                p => p.CategoryId, c => c.Id, (p, c) => new { product = p, category = c })
                .Join(_productBrandQueryRepository.GetQueryable(),
                pc => pc.product.ProductBrandId, b => b.Id, (pc, b) => new { pc.product, pc.category, brand = b })
                .Select(s => new SearchResponseDto
                {
                    produtName = s.product.Name,
                    productBrand = s.brand.Name,
                    categoryName = s.category.Name,
                    Price = s.product.ProductPrices.OrderByDescending(c => c.CreateDate).Select(c => c.Price).FirstOrDefault(),
                    productDetail = s.product.ProductDetails
                                    .Select(c => new ProductDetailResponseDto { Description = c.Description, Size = c.Size, Color = c.Color }).ToList(),
                }).ToListAsync(); */
#endregion


