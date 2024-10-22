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
    public class LogProveedor
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogProveedor _instancia = new LogProveedor();
        //privado para evitar la instanciación directa
        public static LogProveedor Instancia
        {
            get
            {
                return LogProveedor._instancia;
            }
        }
        #endregion singleton

        #region metodos
        ///listado
        public List<EntProveedor> ListarProveedor2()
        {
            return DatProveedor.Instancia.ListarProveedor();
        }

        ///inserta
        public void InsertaProveedor(EntProveedor bus)
        {
            DatProveedor.Instancia.InsertarProveedor(bus);
        }

        public void EditaProveedor(EntProveedor bus)
        {
            DatProveedor.Instancia.EditarProveedor(bus);
        }

        public void DeshabilitarProveedor(EntProveedor bus)
        {
            DatProveedor.Instancia.DeshabilitarProveedor(bus);
        }

        public Boolean ContarTotalProveedor(EntProveedor bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatProveedor.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public DataTable ObtenerProveedorFiltrados(string filtro)
        {
            try
            {
                return DatProveedor.Instancia.FiltrarProveedor(filtro);  // Llamar a la Capa de Datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public DataTable ObtenerProveedorOrdenado2()
        {
            try
            {
                return DatProveedor.Instancia.ObtenerProveedorOrdenado();  // Llamar a la capa de Datos
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