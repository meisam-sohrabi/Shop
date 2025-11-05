namespace ProductService.Api.Helper
{
    public class FileStorage
    {
        public static async Task<string> SaveFileAsync(IFormFile? file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName);
            if (file.Length > 1048576)
            {
                throw new Exception("حجم فایل تصویر بیشتر از ۱ مگابایت است"); // باید درستش کنم مدیریت خطا شون رو اینجا

            }
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("نوع فایل نامعتبر است");
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var url = Path.Combine(uploadFolder, fileName);
            using (var fileStream = new FileStream(url, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return url;
        }
    }
}
