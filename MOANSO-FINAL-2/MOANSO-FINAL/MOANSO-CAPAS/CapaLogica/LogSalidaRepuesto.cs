using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogSalidaRepuesto
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogSalidaRepuesto _instancia = new LogSalidaRepuesto();
        //privado para evitar la instanciación directa
        public static LogSalidaRepuesto Instancia
        {
            get
            {
                return LogSalidaRepuesto._instancia;
            }
        }
        #endregion singleton

        public string GenerarNumeroBoleta2()
        {
            DatSalidaRepuesto ventaDAL = new DatSalidaRepuesto(); // Instanciamos la capa de datos
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
            DatSalidaRepuesto ventaDAL = new DatSalidaRepuesto(); // Instanciamos la capa de datos
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

        public void InsertaSalidaRepuesto(EntSalidaRepuesto bus)
        {
            DatSalidaRepuesto.Instancia.InsertarSalidaRepuesto(bus);
        }

        public void InsertaDetalleSalidaRepuesto(EntSalidaRepuesto bus)
        {
            DatSalidaRepuesto.Instancia.InsertarDetalleSalidaRepuesto(bus);
        }

        public void DeshabilitarSalidaRepuesto(EntSalidaRepuesto bus)
        {
            DatSalidaRepuesto.Instancia.DeshabilitarSalidaRepuesto(bus);
        }

        public List<string> ObtenerCodigosBus(string codigoOP)
        {
            try
            {
                return DatSalidaRepuesto.Instancia.ObtenerCodigosBus(codigoOP);
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
                return DatSalidaRepuesto.Instancia.ObtenerDatosBus(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }
    }
}
