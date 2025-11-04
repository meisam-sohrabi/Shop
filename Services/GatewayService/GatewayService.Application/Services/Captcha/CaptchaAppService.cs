using GatewayService.ApplicationContract.DTO.Base;
using GatewayService.ApplicationContract.DTO.Captcha;
using GatewayService.ApplicationContract.DTO.Token;
using GatewayService.ApplicationContract.Interfaces.Captcha;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
namespace GatewayService.Application.Services.Captcha
{


    #region خلاصه کل کلاس
    /// <summary>
    /// کلاس CaptchaAppService
    /// -----------------------
    /// این کلاس مسئول تولید (کپچا)است تا هنگام ورود کاربر، 
    /// تلاش‌های متعدد غیرمجاز را محدود کند و امنیت سیستم را افزایش دهد.
    /// کپچا شامل یک رشته تصادفی از حروف و اعداد است و به صورت تصویر 
    /// با نویز و خطوط تصادفی تولید می‌شود تا خواندن توسط ربات‌ها سخت باشد.
    ///
    /// خصوصیات:
    /// - استفاده از (رندوم) برای تولید رشته و نویز تصویر
    /// - تولید تصویر Bitmap با اندازه ثابت (220x80)
    /// - اضافه کردن خطوط و پیکسل‌های تصادفی برای افزایش امنیت
    /// - اعمال چرخش و تغییر موقعیت روی هر کاراکتر برای سخت‌تر کردن OCR
    /// - تبدیل تصویر به Base64 جهت ارسال به کلاینت
    /// 
    /// توضیح متدها:
    /// - GenerateCaptcha(int length = 6):
    ///   متد اصلی برای تولید کپچا. طول کپچا پیش‌فرض 6 کاراکتر است. 
    ///   این متد:
    ///     1. رشته کپچا را با استفاده از حروف و اعداد تصادفی تولید می‌کند
    ///     2. یک تصویر Bitmap سفید می‌سازد
    ///     3. خطوط تصادفی برای جلوگیری از OCR رسم می‌کند
    ///     4. کاراکترهای کپچا را با چرخش و موقعیت تصادفی روی تصویر رسم می‌کند
    ///     5. نویز پیکسل‌های رنگی به تصویر اضافه می‌کند
    ///     6. تصویر را به فرمت JPEG ذخیره و به Base64 تبدیل می‌کند
    ///     7. کد و تصویر Base64 را در یک DTO بازمی‌گرداند
    /// 
    /// مثال استفاده:
    /// var captcha = _captchaAppService.GenerateCaptcha();
    /// var base64Image = captcha.ImageBase64; // برای نمایش در frontend
    /// var code = captcha.Code; // برای ذخیره در Session و بررسی بعدی
    /// </summary>
    #endregion
    public class CaptchaAppService : ICaptchaAppService
    {
        private static readonly Random _rand = new Random();
        private readonly IHttpContextAccessor _httpContext;
        private const string _chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        public CaptchaAppService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }


        #region Generate-Captcha


        /// <summary>
        /// تولید یک کپچا به صورت تصویر و کد متناظر
        /// </summary>
        /// <param name="length">طول رشته کپچا (پیش‌فرض 6)</param>
        /// <returns>CaptchaResponseDto شامل Code و ImageBase64</returns>
        public CaptchaResponseDto GenerateCaptcha(int length = 6)
        {
            // تولید رشته تصادفی کپچا
            var code = GenerateCode(length);


            // ساخت تصویر Bitmap با اندازه ثابت
            using var bmp = new Bitmap(220, 80);
            using var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.White); // زمینه سفید


            // تنظیم قلم و قلمو برای رسم کاراکترها
            using var font = new Font("Arial", 36, FontStyle.Bold);
            using var brush = new SolidBrush(Color.Black);


            // رسم خطوط تصادفی برای افزایش سختی خواندن OCR
            for (int i = 0; i < 8; i++)
            {
                var pen = new Pen(Color.FromArgb(_rand.Next(150), _rand.Next(150), _rand.Next(150)), 2);
                gr.DrawLine(pen, _rand.Next(0, 220), _rand.Next(0, 80), _rand.Next(0, 220), _rand.Next(0, 80));
            }


            // رسم کاراکترهای کپچا با چرخش و موقعیت تصادفی
            int x = 10;
            foreach (char c in code)
            {
                using var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddString(c.ToString(), font.FontFamily, (int)FontStyle.Bold, 36, new PointF(x, _rand.Next(10, 30)), StringFormat.GenericDefault);
                float angle = _rand.Next(-25, 25);
                var matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.RotateAt(angle, new PointF(x + 20, 40));
                path.Transform(matrix);
                gr.FillPath(brush, path);
                x += 30;
            }

            // اضافه کردن نویز پیکسلی تصادفی برای سخت‌تر کردن OCR
            for (int i = 0; i < 300; i++)
            {
                bmp.SetPixel(_rand.Next(220), _rand.Next(80), Color.FromArgb(_rand.Next(255), _rand.Next(255), _rand.Next(255)));
            }


            // تبدیل تصویر به Base64
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            var base64 = Convert.ToBase64String(ms.ToArray());


            // بازگرداندن کد کپچا و تصویر Base64
            return new CaptchaResponseDto
            {
                Code = code,
                ImageBase64 = base64,
            };
        }

        #endregion

        #region GenerateCode

        // متد ساخت کد تصادفی 
        private string GenerateCode(int length)
        {
            char[] code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = _chars[_rand.Next(_chars.Length)];
            }
            return new string(code);

        }

        #endregion


        #region VerifyCaptcah

        /// <summary>
        /// متد VerifyCaptcha
        /// -----------------
        /// این متد مسئول بررسی صحت کپچا وارد شده توسط کاربر است.
        /// فرآیند کار:
        /// 1. رشته وارد شده توسط کاربر (کد) با مقدار کپچای ذخیره شده در (سشن) مقایسه می‌شود.
        /// 2. اگر مقادیر برابر باشند:
        ///    - سشن مقدار (کپچای تایید شده) را به "درست" تغییر می‌دهد.
        ///    - یک پاسخ موفق با پیام مناسب و داده (درست) بازگردانده می‌شود.
        /// 3. اگر مقادیر برابر نباشند:
        ///    - سشن مقدار (کپچای تایید شده) را به (نادرست) تغییر می‌دهد.
        ///    - پاسخ با پیام خطا و داده (نادرست) بازگردانده می‌شود.
        ///
        /// نکات مهم:
        /// - این متد فقط وضعیت کپچا را بررسی و در سشن ذخیره می‌کند.
        /// - لاگین) باید بررسی کند که (کپچای تایید شده) برابر ("درست") باشد تا اجازه ورود بدهد).
        /// - این روش اجازه می‌دهد کاربر کپچا را قبل از ورود یا بعد از چند تلاش ناموفق تأیید کند.
        ///
        /// پارامترها:
        /// - code: رشته کپچای وارد شده توسط کاربر
        ///
        /// مقدار بازگشتی:
        /// - BaseResponseDto<bool>:
        ///    - Data = true → کپچا صحیح است
        ///    - Data = false → کپچا اشتباه است
        /// </summary>
        public BaseResponseDto<bool> VerifyCaptcha(string code)
        {
            var session = _httpContext.HttpContext?.Session;

            // بررسی اینکه رشته وارد شده با کپچا ذخیره شده در (سشن) برابر است یا خیر
            if (session.GetString("CaptchaCode")?.Equals(code) ?? false)
            {
                // کپچا درست است → علامت گذاری در سشن
                session.SetString("VerifiedCaptcha", "True");
                return new BaseResponseDto<bool>
                {
                    Message = "کپچا با موفقیت تایید شد",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = true
                };
            }


            // کپچا اشتباه است → علامت گذاری در سشن
            session.SetString("VerifiedCaptcha", "False");
            return new BaseResponseDto<bool>
            {
                Message = "کپچا مورد تایید نمی باشد ",
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Data = false
            };
        }
        #endregion

        #region RefreshCaptcha

        /// <summary>
        /// متد RefreshCaptcha (رفرش کپچا)
        /// --------------------------------
        /// این متد مسئول تولید دوباره کپچا (کپچا) برای کاربر است.  
        /// معمولاً زمانی استفاده می‌شود که کاربر نمی‌تواند کپچای فعلی را بخواند یا قصد دارد یک کپچای جدید دریافت کند.
        ///
        /// فرآیند کار:
        /// 1. یک کپچا (کپچا) جدید توسط (متد ایجاد کپچا) تولید می‌شود.
        /// 2. کد کپچا (کد کپچا) در جلسه (سشن) ذخیره می‌شود.
        /// 3. تصویر کپچا به صورت (بیس64) در پاسخ برگردانده می‌شود تا در رابط کاربری (فرانت اند) نمایش داده شود.
        ///
        /// نکات مهم:
        /// -  
        ///   بررسی وضعیت تأیید کپچا  فقط در متد (تایید کپچا) انجام می‌شود.
        /// - تعداد تلاش‌های ناموفق  تحت تأثیر رفرش کپچا قرار نمی‌گیرد.
        /// - این متد فقط تجربه کاربری (یو ایکس) را بهبود می‌دهد و امنیت سیستم را تحت تأثیر منفی قرار نمی‌دهد.
        ///
        /// مقدار بازگشتی:
        /// - BaseResponseDto<TokenDto>:
        ///    - Data.ImageUrl: تصویر کپچا جدید به صورت Base64
        ///    - Success: true در صورت تولید موفق
        ///    - Message: پیام مناسب برای کاربر
        /// </summary>
        public BaseResponseDto<TokenDto> RefreshCaptcha()
        {
            var session = _httpContext.HttpContext.Session;
            var captcha = GenerateCaptcha();
            session.SetString("CaptchaCode", captcha.Code);

            return new BaseResponseDto<TokenDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = "کپچا رفرش شد",
                Data = new TokenDto { ImageUrl = captcha.ImageBase64 }
            };
        }
        #endregion

    }
}
