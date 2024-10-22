using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogOTE
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogOTE _instancia = new LogOTE();
        //privado para evitar la instanciación directa
        public static LogOTE Instancia
        {
            get
            {
                return LogOTE._instancia;
            }
        }
        #endregion singleton

        public string GenerarNumeroBoleta2()
        {
            DatOTE ventaDAL = new DatOTE(); // Instanciamos la capa de datos
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
            DatOTE ventaDAL = new DatOTE(); // Instanciamos la capa de datos
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

        public List<EntRepuesto> ListarRepuesto2()
        {
            return DatOTE.Instancia.ListarRepuesto();
        }

        public void InsertaOTE(EntOTE bus)
        {
            DatOTE.Instancia.InsertarOTE(bus);
        }

        public void InsertaDetalleOTE(EntOTE bus)
        {
            DatOTE.Instancia.InsertarDetalleOTE(bus);
        }

        public void DeshabilitarOTE(EntOTE bus)
        {
            DatOTE.Instancia.DeshabilitarOTE(bus);
        }

        public List<string> ObtenerCodigosBus(string codigoCM)
        {
            try
            {
                return DatOTE.Instancia.ObtenerCodigosBus(codigoCM);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de buses en la capa lógica: " + ex.Message);
            }
        }

        // Método para obtener los códigos de los proveedores
        public List<string> ObtenerCodigosProveedor(string codigoCM)
        {
            try
            {
                return DatOTE.Instancia.ObtenerCodigosProveedor(codigoCM);
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
                return DatOTE.Instancia.ObtenerDatosProve(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }

        public EntBus ObtenerDatosBus(string codigoBus)
        {
            try
            {
                return DatOTE.Instancia.ObtenerDatosBus(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }

    }
}
