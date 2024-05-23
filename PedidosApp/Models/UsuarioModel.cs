using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Usuarios")]
    public class UsuarioModel
    {
        [Key]
        [Required]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "DNI")]
        public string Dni { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        public int Id_Direccion { get; set; }

        public string Cod_Cliente { get; set; }

        public int Id_Rol {  get; set; }

        [ForeignKey ("Id_Rol")]
        public virtual RolModel Rol { get; set; }

        [NotMapped]
        public string Calle { get; set; }

        [NotMapped]
        public int Numero { get; set; }

        [NotMapped]
        public int Id_Provincia { get; set; }

        [NotMapped]
        public int Id_Localidad { get; set; }

        [ForeignKey("Id_Direccion")]
        public virtual DireccionModel Direccion { get; set; }
    }
}
