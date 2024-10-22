using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogOrdenPedido
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogOrdenPedido _instancia = new LogOrdenPedido();
        //privado para evitar la instanciación directa
        public static LogOrdenPedido Instancia
        {
            get
            {
                return LogOrdenPedido._instancia;
            }
        }
        #endregion singleton


        public string GenerarNumeroBoleta2()
        {
            DatOrdenPedido ventaDAL = new DatOrdenPedido(); // Instanciamos la capa de datos
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
            DatOrdenPedido ventaDAL = new DatOrdenPedido(); // Instanciamos la capa de datos
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

        public void InsertaOrdenPedido(EntOrdenPedido bus)
        {
            DatOrdenPedido.Instancia.InsertarOrdenPedido(bus);
        }

        public void InsertaDetalleOrdenPedido(EntOrdenPedido bus)
        {
            DatOrdenPedido.Instancia.InsertarDetalleOrdenPedido(bus);
        }

        public void DeshabilitarOrdenPedido(EntOrdenPedido bus)
        {
            DatOrdenPedido.Instancia.DeshabilitarOrdenPedido(bus);
        }
    }
}
