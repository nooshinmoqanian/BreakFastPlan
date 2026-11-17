using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO
{
    public class RegisterDto
    {
        [StringLength(25, MinimumLength = 3, ErrorMessage = "نام کاربری باید بین ۳ تا ۲۵ حرف باشد")]
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "رمز عبور باید بین ۸ تا ۲۰ حرف باشد")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن یکسان نیستند")]
        public string passwordConfirm { get; set; } = string.Empty;

        [Required(ErrorMessage = "ایمیل را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string Email { get; set; } = string.Empty;
    }
}
