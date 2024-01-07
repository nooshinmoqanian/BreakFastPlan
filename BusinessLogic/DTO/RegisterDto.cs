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
        [StringLength(25, MinimumLength = 3, ErrorMessage = " username must be more than 2 characters and less than 25 characters")]
        [Required(ErrorMessage = "username can not empty!")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "password can not empty!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The password must be more than 8 characters and less than 30 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "passwordConfirm can not empty!")]
        [Compare("Password")]
        public string passwordConfirm { get; set; } = string.Empty;

        [Required(ErrorMessage = "email can not empty!")]
        [EmailAddress(ErrorMessage = "The email is not valid")]
        public string Email { get; set; } = string.Empty;
    }
}
