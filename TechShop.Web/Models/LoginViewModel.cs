using System.ComponentModel.DataAnnotations;

namespace TechShop.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe indicar el usuario")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = "";

        [Required(ErrorMessage = "Debe indicar la contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = "";
    }
}
