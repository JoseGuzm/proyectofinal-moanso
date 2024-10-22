using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EntFactura
    {
        public int CodigoFactura { get; set; }  // Corresponds to [CodigoFactura] in SQL

        // Propiedad Fecha de Factura (DateTime)
        public DateTime Fecha { get; set; }  // Corresponds to [Fecha] in SQL

        // Propiedad Código Proveedor (string)
        public string CodigoPro { get; set; }  // Corresponds to [CodigoPro] in SQL

        // Propiedad Forma de Pago (string)
        public string FormaPago { get; set; }  // Corresponds to [FormaPago] in SQL

        // Propiedad Código Repuesto (string)
        public string CodigoRep { get; set; }  // Corresponds to [CodigoRep] in SQL

        // Propiedad Cantidad (int)
        public int Cantidad { get; set; }  // Corresponds to [Cantidad] in SQL

        // Propiedad Precio (decimal)
        public decimal Precio { get; set; }  // Corresponds to [Precio] in SQL

        // Propiedad Nombre de Empresa (string)
        public string NombreEmpresa { get; set; }  // Corresponds to [NombreEmpresa] in SQL

        // Propiedad Razón Social (string)
        public string RazonSocial { get; set; }  // Corresponds to [RazonSocial] in SQL

        // Propiedad RUC (string)
        public string RUC { get; set; }  // Corresponds to [RUC] in SQL

        // Propiedad Dirección (string)
        public string Direccion { get; set; }  // Corresponds to [Direccion] in SQL

        // Propiedad Teléfono (string)
        public string Telefono { get; set; }  // Corresponds to [Telefono] in SQL

        // Propiedad Descripción de Repuesto (string)
        public string DescripcionRepuesto { get; set; }  // Corresponds to [DescripcionRepuesto] in SQL

        // Propiedad Categoría del Repuesto (string)
        public string CategoriaR { get; set; }  // Corresponds to [CategoriaR] in SQL

        // Propiedad Marca del Repuesto (string)
        public string MarcarepuestoR { get; set; }

        public string CodigoOC { get; set; }
        public double TOTAL { get; set; }
    }
}
