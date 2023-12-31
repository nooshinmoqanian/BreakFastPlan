﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
        public string? AccessToken { get; set; } = string.Empty;
    }
}
