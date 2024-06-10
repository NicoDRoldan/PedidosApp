using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Categorias")]
    public class CategoriaModel
    {
        [Key]
        public int Id_Categoria { get; set; }

        public string Nombre { get; set; }

        public virtual ICollection<Articulos_CategoriasModel> Articulos_Categorias { get; set; }
    }
}
