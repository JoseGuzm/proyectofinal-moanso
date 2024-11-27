using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntRepuesto
    {
        public string Codigo { get; set; }
        public string Categoria { get; set; }
        public string Marcarepuesto { get; set; }
        public string Descripcion { get; set; }
        public string Proveedor { get; set; }
        public DateTime FechaAdquisicion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int Stock { get; set; }
        public string Estado { get; set; }
    }
}
