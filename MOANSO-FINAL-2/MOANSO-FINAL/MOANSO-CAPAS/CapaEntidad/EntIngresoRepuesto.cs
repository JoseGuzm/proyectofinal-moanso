using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntIngresoRepuesto
    {
        public string CodigoIR { get; set; }       // Corresponds to [CodigoIR] in SQL
        public string CodigoOC { get; set; }       // Corresponds to [CodigoOC] in SQL
        public DateTime Fecha { get; set; }        // Corresponds to [Fecha] in SQL
        public string ProveedorIR { get; set; }    // Corresponds to [ProveedorIR] in SQL
        public string Estado { get; set; }


        public int DetallenotaingresoID { get; set; }   // Corresponds to [DetallenotaingresoID] in SQL (Identity)
        public string IRCodigo { get; set; }            // Corresponds to [IRCodigo] in SQL
        public int CantidadRecibida { get; set; }       // Corresponds to [CantidadRecibida] in SQL
        public string CodigoRepu { get; set; }          // Corresponds to [CodigoRepu] in SQL
        public int CantidadAceptada { get; set; }       // Corresponds to [CantidadAceptada] in SQL
        public decimal Precio { get; set; }
    }
}
