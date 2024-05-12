using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Articulos")]
    public class ArticuloModel
    {
        [Key]
        public int Id_Articulo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int Id_Rubro { get; set; }

        [ForeignKey("Id_Rubro")]
        public virtual RubroModel Rubro { get; set; }

        public virtual PrecioModel? Precio { get; set; }
    }
}
