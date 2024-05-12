using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Pedidos")]
    public class PedidoModel
    {
        [Key]
        public int NumPedido { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime Hora { get; set; }

        public bool Estado { get; set; }

        public string FormaPago { get; set; }

        public decimal Importe { get; set; }

        public string Nota { get; set; }

        public int Id_Usuario { get; set; }

        public virtual IEnumerable<PedidoDetalleModel> PedidoDetalleModel { get; set; }
    }
}
