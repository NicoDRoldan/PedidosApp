namespace PedidosApp.Models
{
    public class CuponModel
    {
        public int Id_Cupon { get; set; }

        public string Descripcion { get; set; }

        public decimal PorcentajeDto { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string TipoCupon { get; set; }

        public string? Url_Imagen { get; set; }

        public virtual ICollection<CuponCategoriaModel> Cupones_Categorias { get; set; }
    }
}
