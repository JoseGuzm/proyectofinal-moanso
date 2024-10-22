using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;
using System.Data;

namespace CapaLogica
{
    public class LogBus
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogBus _instancia = new LogBus();
        //privado para evitar la instanciación directa
        public static LogBus Instancia
        {
            get
            {
                return LogBus._instancia;
            }
        }
        #endregion singleton

        #region metodos
        ///listado
        public List<EntBus> ListarBus2()
        {
            return DatBus.Instancia.ListarBus();
        }

        ///inserta
        public void InsertaCliente(EntBus bus)
        {
            DatBus.Instancia.InsertarBus(bus);
        }

        public void EditaBus(EntBus bus)
        {
            DatBus.Instancia.EditarBus(bus);
        }

        public void DeshabilitarBus(EntBus bus)
        {
            DatBus.Instancia.DeshabilitarBus(bus);
        }

        public Boolean ContarTotalBuses(EntBus bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatBus.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public DataTable ObtenerClientesFiltrados(string filtro)
        {
            try
            {
                return DatBus.Instancia.FiltrarBus(filtro);  // Llamar a la Capa de Datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public DataTable ObtenerBusOrdenado2()
        {
            try
            {
                return DatBus.Instancia.ObtenerBusOrdenado();  // Llamar a la capa de Datos
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }
        #endregion metodos
    }
}
