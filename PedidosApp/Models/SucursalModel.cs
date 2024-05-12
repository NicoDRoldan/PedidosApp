using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Sucursales")]
    public class SucursalModel
    {
        [Key]
        public int Id_Sucursal { get; set; }

        public string Nombre { get; set; }

        public int Id_TipoSucursal { get; set; }

        public int Id_Direccion { get; set; }
    }
}
