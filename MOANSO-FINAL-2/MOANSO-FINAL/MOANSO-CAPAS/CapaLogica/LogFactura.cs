using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogFactura
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogFactura _instancia = new LogFactura();
        //privado para evitar la instanciación directa
        public static LogFactura Instancia
        {
            get
            {
                return LogFactura._instancia;
            }
        }
        #endregion singleton

        #region metodos
        ///listado
        public List<EntFactura> ListarFactura2()
        {
            return DatFactura.Instancia.ListarFactura();
        }

        public Boolean ContarTotalFactura(EntFactura bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatFactura.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        private DatFactura _facturaData = new DatFactura();

        public EntFactura ObtenerFacturaDetalles(int codigoFactura)
        {
            // Aquí puedes agregar más lógica de negocio si es necesario
            return _facturaData.ObtenerDetallesFactura(codigoFactura);
        }

        private readonly DatFactura _facturaData2;
        public List<EntFactura> ObtenerDetallesRepuesto(int codigoFactura)
        {
            return _facturaData2.ObtenerDetallesRepuesto(codigoFactura);
        }

        #endregion metodos
    }
}
