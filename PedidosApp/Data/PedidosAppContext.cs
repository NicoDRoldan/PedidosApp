using Microsoft.EntityFrameworkCore;
using PedidosApp.Models;

namespace PedidosApp.Data
{
    public class PedidosAppContext : DbContext
    {
        public PedidosAppContext(DbContextOptions<PedidosAppContext> options) : base(options) { }

        public DbSet<PedidosApp.Models.ArticuloModel> Articulos { get; set; }
        public DbSet<PedidosApp.Models.DireccionModel> Direccion { get; set; }
        public DbSet<PedidosApp.Models.LocalidadModel> Localidades { get; set; }
        public DbSet<PedidosApp.Models.PedidoDetalleModel> PedidosDetalle { get; set; }
        public DbSet<PedidosApp.Models.PedidoModel> Pedidos { get; set; }
        public DbSet<PedidosApp.Models.PrecioModel> Precios { get; set; }
        public DbSet<PedidosApp.Models.PromocionDetalleModel> PromocionesDetalles { get; set; }
        public DbSet<PedidosApp.Models.PromocionModel> Promociones { get; set; }
        public DbSet<PedidosApp.Models.ProvinciaModel> Provincias { get; set; }
        public DbSet<PedidosApp.Models.RubroModel> Rubros { get; set; }
        public DbSet<PedidosApp.Models.SucursalModel> Sucursales { get; set; }
        public DbSet<PedidosApp.Models.TipoSucursalModel> TipoSucursal { get; set; }
        public DbSet<PedidosApp.Models.UsuarioModel> Usuarios { get; set; }
        public DbSet<PedidosApp.Models.RolModel> Rol { get; set; }
        public DbSet<PedidosApp.Models.CodigoRecuperacionModel> CodigosRecuperacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoDetalleModel>()
                .HasKey(pd => new { pd.NumPedido, pd.Renglon });

            modelBuilder.Entity<PromocionDetalleModel>()
                .HasKey(pd => new { pd.Id_Promocion, pd.Id_Articulo });

            base.OnModelCreating(modelBuilder);
        }
    }
}
