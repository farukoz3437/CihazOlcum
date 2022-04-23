using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Models
{
    public class UserDetailModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ad boş geçilemez.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad boş geçilemez.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Mail boş geçilemez.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası boş geçilemez.")]
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsDelete { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
