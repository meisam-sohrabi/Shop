using AutoMapper;
using FluentValidation;
using LogService;
using Microsoft.EntityFrameworkCore;
using ProductService.ApplicationContract;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.Category;
using ProductService.ApplicationContract.Interfaces.Category;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Category;
using ProductService.InfrastructureContract.Interfaces.Query.Category;
using System.Net;
using System.Text.Json;
namespace ProductService.Application.Services.Category
{
    public class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly ICategoryCommandRepository _categoryCommandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogAppService _logAppService;
        private readonly IUserAppService _userAppService;
        private readonly IValidator<CategoryDto> _validator;

        public CategoryAppService(ICategoryQueryRepository categoryQueryRepository,
            ICategoryCommandRepository categoryCommandRepository, IUnitOfWork unitOfWork
            , IMapper mapper, ILogAppService logAppService, IUserAppService userAppService
            , IValidator<CategoryDto> validator)
        {
            _categoryQueryRepository = categoryQueryRepository;
            _categoryCommandRepository = categoryCommandRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logAppService = logAppService;
            _userAppService = userAppService;
            _validator = validator;
        }

        #region Create
        public async Task<BaseResponseDto<CategoryDto>> CreateCategory(CategoryDto categoryDto)
        {

            // در این قسمت ولیدیشن صورت میگیره
            var validationResult = await _validator.ValidateAsync(categoryDto);

            var output = new BaseResponseDto<CategoryDto>
            {
                Message = "خطا در درج دسته بندی",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };

            // در این قسمت چک میشه و یک دیکشنری که کلید اسم پراپرتی هستش و ولیو لیستی از خطا
            if (!validationResult.IsValid)
            {
                output.Message = "خطاهای اعتبارسنجی رخ داده است.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                output.ValidationErrors = validationResult.ToDictionary();
                return output;
            }
            var currentUserId = _userAppService.GetCurrentUser();
            var categoryExist = await _categoryQueryRepository.GetQueryable().AnyAsync(c => c.Name == categoryDto.Name);
            if (categoryExist)
            {
                output.Message = "دسته بندی از قبل ثبت شده است";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotAcceptable;
                return output;
            }
            var mapped = _mapper.Map<CategoryEntity>(categoryDto);
            await _categoryCommandRepository.AddAsync(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = $"دسته بندی {categoryDto.Name} با موفقیت درج شد";
                output.Success = true;
            }
            await _logAppService.LogAsync(JsonSerializer.Serialize(mapped), "Category", currentUserId);
            output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Edit
        public async Task<BaseResponseDto<CategoryDto>> EditCategory(int id, CategoryDto categoryDto)
        {
            // در این قسمت ولیدیشن صورت میگیره
            var validationResult = await _validator.ValidateAsync(categoryDto);

            var output = new BaseResponseDto<CategoryDto>
            {
                Message = "خطا در به روز رسانی دسته بندی",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };

            // در این قسمت چک میشه و یک دیکشنری که کلید اسم پراپرتی هستش و ولیو لیستی از خطا
            if (!validationResult.IsValid)
            {
                output.Message = "خطاهای اعتبارسنجی رخ داده است.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                output.ValidationErrors = validationResult.ToDictionary();
                return output;
            }
            var categoryExist = await _categoryQueryRepository.GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (categoryExist == null)
            {
                output.Message = "دسته بندی یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var mapped = _mapper.Map(categoryDto, categoryExist);
            _categoryCommandRepository.Edit(categoryExist);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "دسته بندی با موفقیت به روزرسانی شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Delete
        public async Task<BaseResponseDto<CategoryDto>> DeleteCategory(int id)
        {
            var output = new BaseResponseDto<CategoryDto>
            {
                Message = "خطا در حذف دسته بندی",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var categoryExist = await _categoryQueryRepository.GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoryExist == null)
            {
                output.Message = "دسته‌بندی موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            if (await _categoryQueryRepository.GetQueryable().AnyAsync(c => c.Id == id))
            {
                output.Message = "دسته بندی مورد نظر دارای محصولات می باشد امکان حذف وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                return output;
            }

            _categoryCommandRepository.Delete(categoryExist);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "دسته‌بندی با موفقیت حذف شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region GetAll
        public async Task<BaseResponseDto<List<CategoryDto>>> GetAllCategories()
        {
            var output = new BaseResponseDto<List<CategoryDto>>
            {
                Message = "خطا در بازیابی دسته‌بندی ها",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var categories = await _categoryQueryRepository.GetQueryable()
                .Select(c => new CategoryDto { Name = c.Name, IsActive = c.IsActive })
                .ToListAsync();
            if (!categories.Any())
            {
                output.Message = "دسته‌بندی های موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }

            output.Message = "دسته‌بندی ها با موفقیت دریافت شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            output.Data = categories;
            return output;

        }
        #endregion

        #region Get
        public async Task<BaseResponseDto<CategoryDto>> GetCategory(int id)
        {
            var output = new BaseResponseDto<CategoryDto>
            {
                Message = "خطا در بازیابی دسته‌بندی",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var category = await _categoryQueryRepository.GetQueryable()
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto { Name = c.Name, IsActive = c.IsActive })
                .FirstOrDefaultAsync();
            if (category == null)
            {
                output.Message = "دسته‌بندی موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }

            output.Message = "دسته‌بندی با موفقیت دریافت شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            output.Data = category;

            return output;
        }
        #endregion

    }
}
