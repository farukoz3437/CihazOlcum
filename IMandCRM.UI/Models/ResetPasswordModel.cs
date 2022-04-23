using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Token boş.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Mail boş geçilemez.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş geçilemez.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }
    }
}
