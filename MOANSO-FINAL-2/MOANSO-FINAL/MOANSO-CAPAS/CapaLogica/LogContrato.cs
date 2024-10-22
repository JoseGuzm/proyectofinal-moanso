using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogContrato
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogContrato _instancia = new LogContrato();
        //privado para evitar la instanciación directa
        public static LogContrato Instancia
        {
            get
            {
                return LogContrato._instancia;
            }
        }
        #endregion singleton

        public string GenerarNumeroBoleta()
        {
            DatContrato ventaDAL = new DatContrato(); // Instanciamos la capa de datos
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

        public void InsertaContrato(EntContrato bus)
        {
            DatContrato.Instancia.InsertarContrato(bus);
        }

        public void DeshabilitarContrato(EntContrato bus)
        {
            DatContrato.Instancia.DeshabilitarContrato(bus);
        }

        public Boolean ContarTotalContrato(EntContrato bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatContrato.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public List<string> ObtenerCodigosBuses()
        {
            try
            {
                return DatContrato.Instancia.ObtenerCodigosBuses();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de buses en la capa lógica: " + ex.Message);
            }
        }

        public EntBus ObtenerDatosBus(string codigoBus)
        {
            try
            {
                return DatContrato.Instancia.ObtenerDatosBus(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }

        // Método para obtener los códigos de proveedores
        public List<string> ObtenerCodigosProveedores()
        {
            try
            {
                return DatContrato.Instancia.ObtenerCodigosProveedores();
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
                return DatContrato.Instancia.ObtenerDatosProve(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }
    }
}
