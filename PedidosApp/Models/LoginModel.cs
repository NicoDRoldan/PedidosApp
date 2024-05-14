using System.ComponentModel.DataAnnotations;

namespace PedidosApp.Models
{
    public class LoginModel
    {
        [Required]
        public string User { get; set; }

        [Required]
        public string Password { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}
