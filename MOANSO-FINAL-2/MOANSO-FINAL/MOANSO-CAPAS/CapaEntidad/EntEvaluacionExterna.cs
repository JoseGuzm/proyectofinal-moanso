using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntEvaluacionExterna
    {
        public string CodigoEE { get; set; }   // Evaluation code
        public string CodigoBus { get; set; }  // Bus code (refers to the bus in the evaluation)
        public string ProveedorEE { get; set; } // Provider of the evaluation
        public DateTime Fecha { get; set; }    // Date of the evaluation
        public string TECodigo { get; set; }   // Technician code responsible for the evaluation
        public string Estado { get; set; }
    }
}
