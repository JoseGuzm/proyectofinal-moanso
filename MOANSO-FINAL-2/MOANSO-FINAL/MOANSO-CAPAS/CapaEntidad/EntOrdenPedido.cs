using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntOrdenPedido
    {
        public string CodigoOP { get; set; }        // Order Code
        public DateTime Fecha { get; set; }         // Order Date
        public string TICodigo { get; set; }        // Ticket Code (reference)
        public string JefeEncargado { get; set; }   // Person in charge of the order
        public string Descripcion { get; set; }     // Description of the order
        public string Estado { get; set; }


        public int DetalleordenpedidoID { get; set; } // Detail Order ID (auto-incremented)
        public string OPCodigo { get; set; }         // Order Code reference
        public int Cantidad { get; set; }            // Quantity of items ordered
        public string CodigoRepu { get; set; }
    }
}
