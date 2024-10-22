using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntMecanico
    {
        public string Codigo { get; set; }
        public string Especialidad { get; set; }
        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string Domicilio { get; set; }
        public string Experiencia { get; set; }
        public string Telefono { get; set; }
        public double Sueldo { get; set; }
        public string Turno { get; set; }
        public DateTime FechaContrato { get; set; }
        public string Estado { get; set; }
    }
}
