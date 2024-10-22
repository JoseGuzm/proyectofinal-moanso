using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogEvaluacionInterna
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogEvaluacionInterna _instancia = new LogEvaluacionInterna();
        //privado para evitar la instanciación directa
        public static LogEvaluacionInterna Instancia
        {
            get
            {
                return LogEvaluacionInterna._instancia;
            }
        }
        #endregion singleton

        public string GenerarNumeroBoleta2()
        {
            DatEvaluacionInterna ventaDAL = new DatEvaluacionInterna(); // Instanciamos la capa de datos
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
            DatEvaluacionInterna ventaDAL = new DatEvaluacionInterna(); // Instanciamos la capa de datos
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

        public void InsertaEvaluacionInterna(EntEvaluacionInterna bus)
        {
            DatEvaluacionInterna.Instancia.InsertarEvaluacionInterna(bus);
        }

        public void InsertaDetalleEvaluacionInterna(EntEvaluacionInterna bus)
        {
            DatEvaluacionInterna.Instancia.InsertarDetalleEvaluacionInterna(bus);
        }

        public void DeshabilitarEvaluacionInterna(EntEvaluacionInterna bus)
        {
            DatEvaluacionInterna.Instancia.DeshabilitarEvaluacionInterna(bus);
        }

        public Boolean ContarTotalEvaluacionInterna(EntEvaluacionInterna bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatEvaluacionInterna.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public List<string> ObtenerCodigosBuses(string codigoTI)
        {
            try
            {
                return DatEvaluacionInterna.Instancia.ObtenerCodigosBuses(codigoTI);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de buses en la capa lógica: " + ex.Message);
            }
        }

        // Método para obtener los códigos de mecánicos
        public List<string> ObtenerCodigosMecanicos(string codigoTI)
        {
            try
            {
                return DatEvaluacionInterna.Instancia.ObtenerCodigosMecanicos(codigoTI);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de mecánicos en la capa lógica: " + ex.Message);
            }
        }

        public EntBus ObtenerDatosBus(string codigoBus)
        {
            try
            {
                return DatEvaluacionExterna.Instancia.ObtenerDatosBus(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }

        public EntMecanico ObtenerDatosMecani(string codigoBus)
        {
            try
            {
                return DatEvaluacionInterna.Instancia.ObtenerDatosMeca(codigoBus);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus en la capa lógica: " + ex.Message, ex);
            }
        }
    }
}
