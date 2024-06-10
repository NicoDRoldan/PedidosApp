namespace AdministradorPedidosApp.Models.DTOs
{
    public class ArticuloDTO
    {
        public int Id_Articulo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Id_Rubro { get; set; }
        public string? Url_Imagen { get; set; }
        public RubroDTO Rubro { get; set; }
        public PrecioDTO? Precio { get; set; }
        public List<CategoriaDTO> Categorias { get; set; }
    }
}
