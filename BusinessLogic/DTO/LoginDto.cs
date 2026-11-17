using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO
{
    public class LoginDto
    {
        [StringLength(25, MinimumLength = 3, ErrorMessage = "نام کاربری باید بین ۳ تا ۲۵ حرف باشد")]
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string username { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "رمز عبور باید بین ۸ تا ۲۰ حرف باشد")]
        public string password { get; set; } = string.Empty;
    }

}
