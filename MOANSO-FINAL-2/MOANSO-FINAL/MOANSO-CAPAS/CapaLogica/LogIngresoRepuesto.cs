using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogIngresoRepuesto
    {

        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogIngresoRepuesto _instancia = new LogIngresoRepuesto();
        //privado para evitar la instanciación directa
        public static LogIngresoRepuesto Instancia
        {
            get
            {
                return LogIngresoRepuesto._instancia;
            }
        }
        #endregion singleton

        public string GenerarNumeroBoleta2()
        {
            DatIngresoRepuesto ventaDAL = new DatIngresoRepuesto(); // Instanciamos la capa de datos
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
            DatIngresoRepuesto ventaDAL = new DatIngresoRepuesto(); // Instanciamos la capa de datos
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

        public void InsertaIngresoRepuesto(EntIngresoRepuesto bus)
        {
            DatIngresoRepuesto.Instancia.InsertarIngresoRepuesto(bus);
        }

        public void InsertaDetalleIngresoRepuesto(EntIngresoRepuesto bus)
        {
            DatIngresoRepuesto.Instancia.InsertarDetalleIngresoRepuesto(bus);
        }

        public void DeshabilitarIngresoRepuesto(EntIngresoRepuesto bus)
        {
            DatIngresoRepuesto.Instancia.DeshabilitarIngresoRepuesto(bus);
        }

        public List<string> ObtenerCodigosProveedores(string codigoOC)
        {
            try
            {
                return DatIngresoRepuesto.Instancia.ObtenerCodigosProveedores(codigoOC);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de proveedores en la capa lógica: " + ex.Message);
            }
        }

        public EntProveedor ObtenerDatosProve(string codigoBus)
        {
            try
            {
                return DatIngresoRepuesto.Instancia.ObtenerDatosProve(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }

        public List<RepuestoOrdenCompra> Repuestos(string codigoOC)
        {
            try
            {
                // Llamada al método de la Capa de Datos para obtener los repuestos asociados con la Orden de Compra
                return DatIngresoRepuesto.Instancia.ObtenerRepuestosPorOrdenCompra(codigoOC);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones. Proporcionamos un mensaje claro para identificar el origen del problema
                throw new Exception("Error al obtener los repuestos para la orden de compra con Código OC: " + codigoOC + " en la Capa Lógica: " + ex.Message, ex);
            }
        }
    }
}
