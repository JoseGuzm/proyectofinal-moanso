using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntContrato
    {
        public string Codigo { get; set; }    // Contract Code
        public string Bus { get; set; }       // Associated Bus Code (foreign key)
        public string Fecha { get; set; }       // Date of the contract (as string, but you might want DateTime)
        public string Proveedor { get; set; } // Service provider for the maintenance
        public string Descripcion { get; set; } // Description of the maintenance service
        public double Costo { get; set; }      // Cost of the maintenance (decimal to handle money)
        public string Estado { get; set; }
    }
}
