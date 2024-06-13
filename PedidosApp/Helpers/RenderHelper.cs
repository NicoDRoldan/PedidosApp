using Microsoft.AspNetCore.Html;
using PedidosApp.Data;
using PedidosApp.Models;
using System.Text;

namespace PedidosApp.Helpers
{
    public static class RenderHelper
    {
        public static IHtmlContent RenderArticulosPorRubrosSection(IEnumerable<ArticuloModel> articulos, string nombre, string rubro_categoria, int cantItems, string tipo)
        {
            IEnumerable<ArticuloModel> items = articulos;

            if (tipo == "Rubro")
            {
                items = articulos
                    .Where(a => a.Rubro.Nombre == rubro_categoria)
                    .Take(cantItems)
                    .ToList();
            }
            else if(tipo == "Categoria")
            {
                items = articulos
                    .Where(ac => ac.Articulos_Categorias.Any(ac => ac.Categoria.Nombre == rubro_categoria))
                    .Take(cantItems)
                    .ToList();
            }

            var content = new StringBuilder();

            var cerocerocero = "0.00";

            content.AppendLine($"<section class='container--section'>");
            content.AppendLine($"<h2>{nombre} <a href='#' class='view-all'>Ver todas" +
                $"<img src=\"/logos/arrow-view-more.png\" alt=\"\">" +
                $"</a></h2>");
            content.AppendLine("<div class='container'>");

            rubro_categoria = rubro_categoria.Replace(" ", "").Trim();

            foreach (var item in items)
            {
                content.AppendLine($@"
                    <a onclick='addToCart(""{rubro_categoria}"", {item.Id_Articulo}, ""{item.Nombre}"", {item.Precio.Precio.ToString().Replace(',', '.')})'>
                        <div class='card' id='card-{item.Id_Articulo}-{rubro_categoria}'>
                            <div class='card-content'>
                                <div>
                                    <h3>{item.Nombre}</h3>
                                    <div class='mcd-store-menu-category-item__title mcd-store-menu-category-item__title--is-clamped' id='desc-{item.Id_Articulo}-{rubro_categoria}' data-length='{item.Descripcion.Length}'>{item.Descripcion}</div>
                                    <span class='read-more-container'>
                                        <span class='read-more' id='read-more-{item.Id_Articulo}-{rubro_categoria}' onclick='event.stopPropagation(); toggleDescription(""{item.Id_Articulo}-{rubro_categoria}"")'>Leer más</span>
                                    </span>
                                    <span class='read-less-container'>
                                        <span class='read-less' id='read-less-{item.Id_Articulo}-{rubro_categoria}' onclick='event.stopPropagation(); toggleDescription(""{item.Id_Articulo}-{rubro_categoria}"")'>Leer menos</span>
                                    </span>
                                </div>
                                <div>
                                    <div class='mcd-store-menu-category-item__calories price'>${item.Precio.Precio.ToString(cerocerocero)}</div>
                                </div>
                            </div>
                            <div class='card-image'>
                                <img src='{item.Url_Imagen}' alt='Imagen' class='image-art' data-url='{item.Url_Imagen}'>
                            </div>
                        </div>
                    </a>");
            }

            content.AppendLine("</div>");
            content.AppendLine("</section>");


            return new HtmlString(content.ToString());
        }

        public static IHtmlContent RenderRubrosNav(dynamic viewBagRubrosActivos)
        {
            var content = new StringBuilder();

            content.AppendLine($"<nav class=\"mcd-store-menu-block\">");
            content.AppendLine($"<div class=\"mcd-container\">");
            content.AppendLine($"<div class=\"mcd-store-menu-block__list is-hidden-touch\">");
            content.AppendLine($"<ul class=\"columns is-multiline is-variable is-1\">");

            foreach (var rubro in viewBagRubrosActivos)
            {
                content.AppendLine($@"
                    <li class=""column is-4"">
                        <a href=""/pedidos/mcdelivery/6197a025c6ad5ff2b8abda3f/menu/novedades"" class=""mcd-store-menu-category-item"">
                            <div class=""mcd-store-menu-category-item__image""><img src=""{rubro.Url_Imagen}"" alt=""Imagen de {rubro.Nombre}"" loading=""lazy""></div> <div class=""mcd-store-menu-category-item__text-content"">
                            <div class=""mcd-store-menu-category-item__title mcd-store-menu-category-item__title--is-clamped"">
                                    {rubro.Nombre}
                                </div>
                            </div>
                        </a>
                    </li>
                ");
            }

            content.AppendLine($@"
                        </ul>
                    </div>
                </div>
            </nav>"
            );

            return new HtmlString(content.ToString());
        }
    }
}
