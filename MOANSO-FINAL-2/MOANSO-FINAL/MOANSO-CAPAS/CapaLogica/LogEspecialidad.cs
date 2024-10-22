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
    public class LogEspecialidad
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogEspecialidad _instancia = new LogEspecialidad();
        //privado para evitar la instanciación directa
        public static LogEspecialidad Instancia
        {
            get
            {
                return LogEspecialidad._instancia;
            }
        }
        #endregion singleton

        #region metodos
        ///listado
        public List<EntEspecialidad> ListarEspecialidad2()
        {
            return DatEspecialidad.Instancia.ListarEspecialidad();
        }

        ///inserta
        public void InsertaEspecialidad(EntEspecialidad cat)
        {
            DatEspecialidad.Instancia.InsertarEspecialidad(cat);
        }

        public void EditaEspecialidad(EntEspecialidad cat)
        {
            DatEspecialidad.Instancia.EditarEspecialidad(cat);
        }

        public void DeshabilitarEspecialidad(EntEspecialidad cat)
        {
            DatEspecialidad.Instancia.DeshabilitarEspecialidad(cat);
        }

        public Boolean ContarTotalEspecialidad(EntEspecialidad bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatEspecialidad.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public DataTable ObtenerEspecialidadFiltrados(string filtro)
        {
            try
            {
                return DatEspecialidad.Instancia.FiltrarEspecialidad(filtro);  // Llamar a la Capa de Datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public DataTable ObtenerEspecialidadOrdenado2()
        {
            try
            {
                return DatEspecialidad.Instancia.ObtenerEspecialidadOrdenado();  // Llamar a la capa de Datos
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