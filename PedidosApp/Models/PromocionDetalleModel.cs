using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("PromocionesDetalles")]
    public class PromocionDetalleModel
    {
        public int Id_Promocion { get; set; }

        public int Id_Articulo { get; set; }
    }
}
