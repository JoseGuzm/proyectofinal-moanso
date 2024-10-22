using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntOTI
    {
        public string CodigoTI { get; set; }      // Internal Work Order Code
        public string BusTI { get; set; }         // Bus associated with the internal work order
        public DateTime Fecha { get; set; }       // Date of the internal work order
        public string Estado { get; set; }


        public int DetalleotiID { get; set; }     // Detail Internal Work Order ID
        public string OrdentrabajointernoID { get; set; }  // Internal Work Order Code reference
        public string CodigoRepu { get; set; }    // Replacement part/product code
        public string MecanicoTI { get; set; }    // Mechanic assigned to the internal work order
        public string Parte { get; set; }         // Part being worked on or replaced
        public string Pieza { get; set; }         // Piece being worked on or replaced
        public int Cantidad { get; set; }
    }
}
