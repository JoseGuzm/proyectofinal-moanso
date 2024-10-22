using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntEvaluacionInterna
    {
        public string CodigoEI { get; set; }       // Codigo de la Evaluación Interna
        public string CodigoBus { get; set; }      // Codigo del Bus evaluado
        public DateTime Fecha { get; set; }        // Fecha de la Evaluación Interna
        public string TICodigo { get; set; }       // Codigo del Técnico que realizó la evaluación
        public string Estado { get; set; }
        public string EICodigo { get; set; }                // Codigo de la evaluación interna (relacionado con EvaluacionInterna)
        public string MecanicoEI { get; set; }
    }
}
