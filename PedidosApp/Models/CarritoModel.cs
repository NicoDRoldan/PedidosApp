namespace PedidosApp.Models
{
    public class CarritoModel
    {
        public int Id_Articulo { get; set; }

        public string NombreArticulo { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Total { get; set; }
    }
}
