using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntSalidaRepuesto
    {
        public string CodigoSR { get; set; }
        public string BusSR { get; set; }
        public DateTime Fecha { get; set; }
        public string OPCodigo { get; set; }
        public string Estado { get; set; }


        public int DetallenotasalidaID { get; set; }  // Identity column, auto-incremented in DB
        public string DSRCodigo { get; set; }  // Refers to the CodigoSR from NotaSalidaRepuesto
        public int CantidadRecibida { get; set; }
        public string CodigoRepu { get; set; }  // Repuesto (spare part) code
        public int CantidadEnviada { get; set; }
    }
}
