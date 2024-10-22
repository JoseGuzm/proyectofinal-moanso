using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntOTE
    {
        public string CodigoTE { get; set; }      // External Work Order Code
        public string CodigoBus { get; set; }     // Bus associated with the external work order
        public string ContratoCO { get; set; }    // Contract associated with the external work order
        public DateTime Fecha { get; set; }       // Date of the external work order
        public string ProveedorTE { get; set; }   // External work provider
        public string Estado { get; set; }


        public int DetalleoteID { get; set; }     // Detail Work Order ID (auto-incremented)
        public string TECodigo { get; set; }      // External Work Order Code reference
        public string CodigoRepu { get; set; }    // Replacement part/product code
        public string Parte { get; set; }         // Part of the replacement or service
        public string Pieza { get; set; }         // Piece being worked on
        public int Cantidad { get; set; }
    }
}
