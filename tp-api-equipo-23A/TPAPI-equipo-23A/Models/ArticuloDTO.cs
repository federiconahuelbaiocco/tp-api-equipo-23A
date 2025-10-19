using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPAPI_equipo_23A.Models
{
	public class ArticuloDTO
	{
		public string CodigoArticuloDTO { get; set; }
		public string NombreArticuloDTO { get; set; }
		public string DescripcionArticuloDTO { get; set; }
		public decimal PrecioArticuloDTO { get; set; }
		public int IdMarcaDTO { get; set; }
		public int IdCategoriaDTO { get; set; }
		public string UrlImagenDTO { get; set; }
	}
}