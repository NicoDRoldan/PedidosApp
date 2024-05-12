using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("TipoSucursal")]
    public class TipoSucursalModel
    {
        [Key]
        public int Id_Sucursal { get; set; }

        public string Nombre { get; set; }
    }
}
