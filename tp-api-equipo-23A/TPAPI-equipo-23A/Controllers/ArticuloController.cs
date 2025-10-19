using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dominio;
using negocio;
using TPAPI_equipo_23A.Models;

namespace TPAPI_equipo_23A.Controllers 
{ 
    public class ArticuloController : ApiController
    {
        // GET: api/Articulo
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            return negocio.listar();
        }

        // GET: api/Articulo/5
        public Articulo Get(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> lista = negocio.listar();

            return lista.Find(x => x.Id == id);
        }

		// POST: api/Articulo
		public HttpResponseMessage Post([FromBody] ArticuloDTO dto)
		{
			try
			{
				if (dto == null)
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Los datos del artículo no pueden ser nulos o están vacíos.");

				MarcaNegocio marcaNegocio = new MarcaNegocio();
				var marcaExiste = marcaNegocio.listar().Any(m => m.Id == dto.IdMarcaDTO);
				if (!marcaExiste)
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La marca con el ID proporcionado no existe.");

				CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
				var categoriaExiste = categoriaNegocio.listar().Any(c => c.Id == dto.IdCategoriaDTO);
				if (!categoriaExiste)
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La categoría con el ID proporcionado no existe.");

				var articulo = new dominio.Articulo
				{
					Codigo = dto.CodigoArticuloDTO,
					Nombre = dto.NombreArticuloDTO,
					Descripcion = dto.DescripcionArticuloDTO,
					Marca = new dominio.Marca { Id = dto.IdMarcaDTO },
					Categoria = new dominio.Categoria { Id = dto.IdCategoriaDTO },
					Precio = dto.PrecioArticuloDTO,
					listaImagenes = new List<dominio.Imagen>()
				};

				if (!string.IsNullOrWhiteSpace(dto.UrlImagenDTO))
				{
					articulo.listaImagenes.Add(new dominio.Imagen { ImagenUrl = dto.UrlImagenDTO });
				}
				var negocio = new negocio.ArticuloNegocio();
				negocio.agregar(articulo);

				return Request.CreateResponse(HttpStatusCode.Created, "Artículo agregado correctamente.");

			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado al intentar agregar el artículo.", ex);
			}
		}

		// Patch: api/Articulo/5
		public HttpResponseMessage Patch(int id, [FromBody] ImagenesDTO nuevasImagenes)
		{
			try
			{
				ArticuloNegocio articuloNegocio = new ArticuloNegocio();
				var articulo = articuloNegocio.listar().FirstOrDefault(a => a.Id == id);

				if (articulo == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El artículo con el ID " + id + " no existe.");
				}

				if (nuevasImagenes == null || nuevasImagenes.Urls == null || !nuevasImagenes.Urls.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La lista de URLs de imágenes no puede estar vacía.");
				}

				ImagenNegocio imagenNegocio = new ImagenNegocio();
				foreach (var url in nuevasImagenes.Urls)
				{
					if (!string.IsNullOrWhiteSpace(url))
					{
						if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
						{
							return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La URL '" + url + "' no tiene un formato válido.");
						}
						if (articulo.listaImagenes.Any(img => img.ImagenUrl.Equals(url, StringComparison.OrdinalIgnoreCase)))
						{
							return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La URL '" + url + "' ya existe para este artículo.");
						}

						var nuevaImagen = new dominio.Imagen
						{
							IdArticulo = id,
							ImagenUrl = url
						};
						imagenNegocio.agregar(nuevaImagen);
					}
				}

				return Request.CreateResponse(HttpStatusCode.OK, "Imágenes agregadas correctamente al artículo " + id);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado al agregar las imágenes.", ex);
			}
		}

		// PUT: api/Articulo/5
		public void Put(int id, [FromBody] Articulo art)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            art.Id = id;
            negocio.modificar(art);
        }

        // DELETE: api/Articulo/5
        public void Delete(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            negocio.eliminar(id);
        }
    }
}
