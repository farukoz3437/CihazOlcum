using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Mail adresi boş geçilemez.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçerli bir Email adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş geçilemez.")]
        [DataType(DataType.Password, ErrorMessage = "Geçerli bir şifre giriniz.")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
