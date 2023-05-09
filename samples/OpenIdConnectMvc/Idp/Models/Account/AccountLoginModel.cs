using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Models
{
    public class AccountLoginModel
    {
        [FromForm]
        [FromQuery]
        public string? ReturnUrl { get; set; }
        [FromForm]
        [Required]
        public string Username { get; set; } = default!;
        [FromForm]
        [Required]
        public string Password { get; set; } = default!;
    }
}
