using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogOTI
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogOTI _instancia = new LogOTI();
        //privado para evitar la instanciación directa
        public static LogOTI Instancia
        {
            get
            {
                return LogOTI._instancia;
            }
        }
        #endregion singleton

        public string GenerarNumeroBoleta()
        {
            DatOTI ventaDAL = new DatOTI(); // Instanciamos la capa de datos
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

        public void InsertaOTI(EntOTI bus)
        {
            DatOTI.Instancia.InsertarOTI(bus);
        }

        public void InsertaDetalleOTI(EntOTI bus)
        {
            DatOTI.Instancia.InsertarDetalleOTI(bus);
        }

        public void DeshabilitarOTI(EntOTI bus)
        {
            DatOTI.Instancia.DeshabilitarOTI(bus);
        }

        public List<string> ObtenerCodigosBus()
        {
            try
            {
                return DatOTI.Instancia.ObtenerCodigosBus();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de buses en la capa lógica: " + ex.Message);
            }
        }

        public List<string> ObtenerCodigosMecanico()
        {
            try
            {
                return DatOTI.Instancia.ObtenerCodigosMecanico();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de mecánicos en la capa lógica: " + ex.Message);
            }
        }

        public EntMecanico ObtenerDatosMecani(string codigoBus)
        {
            try
            {
                return DatOTI.Instancia.ObtenerDatosMeca(codigoBus);
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
                return DatOTI.Instancia.ObtenerDatosBus(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }

    }
}