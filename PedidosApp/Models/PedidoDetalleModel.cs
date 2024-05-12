using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("PedidosDetalle")]
    public class PedidoDetalleModel
    {
        [Key]
        public string NumPedido { get; set; }

        [Key]
        public int Renglon { get; set; }

        public int Id_Articulo { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnidad { get; set; }

        public decimal PrecioTotal { get; set; }
    }
}
