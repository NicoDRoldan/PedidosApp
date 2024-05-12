using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Promociones")]
    public class PromocionModel
    {
        [Key]
        public int Id_Promocion { get; set; }

        public string Descripcion { get; set; }

        public decimal Porcentaje { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public virtual IEnumerable<PromocionModel> Promociones { get; set;}
    }
}
