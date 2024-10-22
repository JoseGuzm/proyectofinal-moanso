using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntOrdenCompra
    {
        public string CodigoOC { get; set; }          
        public string CodigoPro { get; set; }        
        public DateTime Fecha { get; set; }          
        public string OPCodigo { get; set; }        
        public string FormaPago { get; set; }       
        public decimal Total { get; set; }         
        public string Estado { get; set; }


        public int DetalleordencompraID { get; set; } 
        public string OCCompra { get; set; }      
        public int Cantidad { get; set; }          
        public string CodigoRep { get; set; }     
        public decimal Precio { get; set; }
    }
}
