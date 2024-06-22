using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PedidosApp.Models
{
    public class CuponCategoriaModel
    {
        public int Id_Cupones_Categorias { get; set; }

        public int Id_Cupon { get; set; }

        public int Id_Categoria { get; set; }

        [ForeignKey("Id_Cupon")]
        public virtual CuponModel Cupon { get; set; }

        [ForeignKey("Id_Categoria")]
        public virtual CCuponCategoriaModel Categoria { get; set; }
    }
}
