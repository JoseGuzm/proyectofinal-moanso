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
    public class LogCategoria
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogCategoria _instancia = new LogCategoria();
        //privado para evitar la instanciación directa
        public static LogCategoria Instancia
        {
            get
            {
                return LogCategoria._instancia;
            }
        }
        #endregion singleton

        #region metodos
        ///listado
        public List<EntCategoria> ListarCategoria2()
        {
            return DatCategoria.Instancia.ListarCategoria();
        }

        ///inserta
        public void InsertaCategoria(EntCategoria cat)
        {
            DatCategoria.Instancia.InsertarCategoria(cat);
        }

        public void EditaCategoria(EntCategoria cat)
        {
            DatCategoria.Instancia.EditarCategoria(cat);
        }

        public void DeshabilitarCategoria(EntCategoria cat)
        {
            DatCategoria.Instancia.DeshabilitarCategoria(cat);
        }

        public Boolean ContarTotalCategoria(EntCategoria bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatCategoria.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public DataTable ObtenerCategoriaFiltrados(string filtro)
        {
            try
            {
                return DatCategoria.Instancia.FiltrarCategoria(filtro);  // Llamar a la Capa de Datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public DataTable ObtenerCategoriaOrdenado2()
        {
            try
            {
                return DatCategoria.Instancia.ObtenerCategoriaOrdenado();  // Llamar a la capa de Datos
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