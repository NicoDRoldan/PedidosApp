﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosApp.Models
{
    [Table("Precios")]
    public class PrecioModel
    {
        [Key]
        public int Id_Articulo { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [ForeignKey("Id_Articulo")]
        public virtual ArticuloModel Articulo { get; set; }
    }
}
