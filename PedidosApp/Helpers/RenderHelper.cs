using Microsoft.AspNetCore.Html;
using PedidosApp.Data;
using PedidosApp.Models;
using System.Security.Policy;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PedidosApp.Helpers
{
    public static class RenderHelper
    {
        public static IHtmlContent RenderArticulosPorRubrosSection(IEnumerable<ArticuloModel> articulos, string id_rubro_categoria, string rubro_categoria, int cantItems, string tipo)
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
            content.AppendLine($"<h2>{rubro_categoria} <a href='/Home/Menu/{id_rubro_categoria}/{tipo}' class='view-all'>Ver todas" +
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
                                    <div class='mcd-store-menu-category-item__title data-art-toggle mcd-store-menu-category-item__title--is-clamped' id='desc-{item.Id_Articulo}-{rubro_categoria}' data-length='{item.Descripcion.Length}'>{item.Descripcion}</div>
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
            content.AppendLine($"<ul class=\"columns is-multiline is-variable is-1\" id=\"menuList\">");

            foreach (var rubro in viewBagRubrosActivos)
            {
                content.AppendLine($@"
                    <li class=""column is-4"">
                        <a href=""/Home/Menu/{rubro.Id_Rubro}/rubro"" class=""mcd-store-menu-category-item"">
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
                        <div class=""toggle-button"">
                            <span id=""toggleMenu"">Mostrar más</span>
                        </div>
                    </div>
                </div>
            </nav>"
            );

            return new HtmlString(content.ToString());
        }

        public static IHtmlContent RenderArticulos<T>(IEnumerable<ArticuloModel> articulos, IEnumerable<T> rubro_categoriaModel)
        {
            var content = new StringBuilder();

            var cerocerocero = "0.00";

            var rubro_categoria = "";

            if(typeof(T) == typeof(RubroModel))
            {
                rubro_categoria = rubro_categoriaModel.Cast<RubroModel>().Select(result => result.Nombre).FirstOrDefault();
            }
            else if (typeof(T) == typeof(CategoriaModel))
            {
                rubro_categoria = rubro_categoriaModel.Cast<CategoriaModel>()
                    .Select(result => result.Articulos_Categorias.FirstOrDefault())
                        .Select(result2 => result2.Categoria.Nombre)
                    .FirstOrDefault();
            }

            content.AppendLine($"<section class='container--section'>");
            content.AppendLine($"<div class='menu--a--render'> <a href=\"/Home/\">Menú </a> > {rubro_categoria}</div>");
            content.AppendLine($"<h2>{rubro_categoria}</h2>");
            content.AppendLine("<div class='container'>");

            rubro_categoria = rubro_categoria.Replace(" ", "").Trim();

            foreach (var item in articulos)
            {
                content.AppendLine($@"
                    <a onclick='addToCart(""{rubro_categoria}"", {item.Id_Articulo}, ""{item.Nombre}"", {item.Precio.Precio.ToString().Replace(',', '.')})'>
                        <div class='card' id='card-{item.Id_Articulo}-{rubro_categoria}'>
                            <div class='card-content'>
                                <div>
                                    <h3>{item.Nombre}</h3>
                                    <div class='mcd-store-menu-category-item__title data-art-toggle mcd-store-menu-category-item__title--is-clamped' id='desc-{item.Id_Articulo}-{rubro_categoria}' data-length='{item.Descripcion.Length}'>{item.Descripcion}</div>
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

        public static IHtmlContent CuponesPorCategorias(IEnumerable<CuponModel> cupones)
        {
            // Traer todas las categorias asociadas a los cupones:
            var categorias = cupones
                .SelectMany(c => c.Cupones_Categorias.Select(cc => cc.Categoria))
                .GroupBy(c => c.Id_Categoria)
                .Select(g => g.First())
                .ToList();

            var content = new StringBuilder();

            var cerocerocero = "0";

            foreach (var categoria in categorias)
            {
                var nomCategoria = categoria.Nombre;

                content.AppendLine($"<section class='container--section'>");
                content.AppendLine($"<h2>{nomCategoria}</h2>");
                content.AppendLine("<div class='container'>");

                nomCategoria = categoria.Nombre.Replace(" ", "");

                content.AppendLine("<div class='carousel'>"); // Carousel container

                foreach (var cupon in cupones.Where(c => c.Cupones_Categorias.Any(cc => cc.Categoria.Id_Categoria == categoria.Id_Categoria)).ToList())
                {
                    content.AppendLine($@"
                    <a>
                        <div class=""card"" id=""card-{cupon.Id_Cupon}"">
                            <div class=""discount-circle"">%{cupon.PorcentajeDto.ToString(cerocerocero)}</div>
                            <div class=""card-image"">
                                <img src=""{cupon.Url_Imagen}"" alt=""Imagen"" class=""image-art"" data-url=""{cupon.Url_Imagen}"">
                            </div>
                            <div class=""card-content"">
                                <div class=""mcd-store-menu-category-item__title"">{cupon.Descripcion}</div>
                            </div>
                        </div>
                    </a>");
                }

                content.AppendLine("</div>"); // Close carousel container
                content.AppendLine("</div>");
                content.AppendLine("</section>");
            }


            return new HtmlString(content.ToString());
        }
    }
}
