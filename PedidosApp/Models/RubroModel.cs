using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Rubros")]
    public class RubroModel
    {
        [Key]
        public int Id_Rubro { get; set; }

        public string Nombre { get; set; }
    }
}
