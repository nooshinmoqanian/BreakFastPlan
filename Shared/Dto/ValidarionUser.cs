using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ValidarionUser
    {
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
