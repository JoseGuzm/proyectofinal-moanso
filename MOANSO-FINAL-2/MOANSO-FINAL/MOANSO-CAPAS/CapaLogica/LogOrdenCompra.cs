using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogOrdenCompra
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogOrdenCompra _instancia = new LogOrdenCompra();
        //privado para evitar la instanciación directa
        public static LogOrdenCompra Instancia
        {
            get
            {
                return LogOrdenCompra._instancia;
            }
        }
        #endregion singleton

        public string GenerarNumeroBoleta2()
        {
            DatOrdenCompra ventaDAL = new DatOrdenCompra(); // Instanciamos la capa de datos
            int reg = ventaDAL.ObtenerNumeroBoleta2(); // Llamamos al método de la capa de datos

            string r = "";

            // Determinamos el prefijo basado en el número de registros
            if (reg >= 1 && reg < 10)
            {
                r = "000";
            }
            else if (reg >= 10 && reg < 100)
            {
                r = "00";
            }
            else if (reg >= 100 && reg < 1000)
            {
                r = "0";
            }

            // Generamos el número de boleta
            return r + reg.ToString();
        }

        public string GenerarNumeroBoleta()
        {
            DatOrdenCompra ventaDAL = new DatOrdenCompra(); // Instanciamos la capa de datos
            int reg = ventaDAL.ObtenerNumeroBoleta(); // Llamamos al método de la capa de datos

            string r = "";

            // Determinamos el prefijo basado en el número de registros
            if (reg >= 1 && reg < 10)
            {
                r = "000";
            }
            else if (reg >= 10 && reg < 100)
            {
                r = "00";
            }
            else if (reg >= 100 && reg < 1000)
            {
                r = "0";
            }

            // Generamos el número de boleta
            return r + reg.ToString();
        }

        public void InsertaOrdenCompra(EntOrdenCompra bus)
        {
            DatOrdenCompra.Instancia.InsertarOrdenCompra(bus);
        }

        public void InsertaDetalleOrdenCompra(EntOrdenCompra bus)
        {
            DatOrdenCompra.Instancia.InsertarDetalleOrdenCompra(bus);
        }

        public void InsertaFactura(EntFactura bus)
        {
            DatOrdenCompra.Instancia.InsertarFactura(bus);
        }

        public void DeshabilitarOrdenCompra(EntOrdenCompra bus)
        {
            DatOrdenCompra.Instancia.DeshabilitarOrdenCompra(bus);
        }

        public List<string> ObtenerProveedores()
        {
            try
            {
                return DatOrdenCompra.Instancia.ObtenerProveedores();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los proveedores en la capa lógica: " + ex.Message);
            }
        }

        public EntProveedor ObtenerDatosProve(string codigoBus)
        {
            try
            {
                return DatOrdenCompra.Instancia.ObtenerDatosProve(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }
    }
}
