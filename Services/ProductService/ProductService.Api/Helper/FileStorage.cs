namespace ProductService.Api.Helper
{
    public class FileStorage
    {
        public static async Task<string> SaveFileAsync(IFormFile? file)
        {

            var fileExtension = Path.GetExtension(file.FileName);
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
