﻿using System.ComponentModel.DataAnnotations;

namespace SmartCalender.API.Models
{

    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }

}
