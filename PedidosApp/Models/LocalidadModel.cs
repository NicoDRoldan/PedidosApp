using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Localidades")]
    public class LocalidadModel
    {
        [Key]
        public int Id_Localidad { get; set; }

        public string Nombre { get; set; }
    }
}
