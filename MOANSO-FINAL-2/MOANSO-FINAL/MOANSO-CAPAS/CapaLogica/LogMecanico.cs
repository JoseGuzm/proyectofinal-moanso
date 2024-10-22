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
    public class LogMecanico
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogMecanico _instancia = new LogMecanico();
        //privado para evitar la instanciación directa
        public static LogMecanico Instancia
        {
            get
            {
                return LogMecanico._instancia;
            }
        }
        #endregion singleton


        #region metodos
        ///listado
        public List<EntMecanico> ListarMecanico2()
        {
            return DatMecanico.Instancia.ListarMecanico();
        }

        ///inserta
        public void InsertaMecanico(EntMecanico bus)
        {
            DatMecanico.Instancia.InsertarMecanico(bus);
        }

        public void EditaMecanico(EntMecanico bus)
        {
            DatMecanico.Instancia.EditarMecanico(bus);
        }

        public void DeshabilitarMecanico(EntMecanico bus)
        {
            DatMecanico.Instancia.DeshabilitarMecanico(bus);
        }

        public Boolean ContarTotalMecanico(EntMecanico bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatMecanico.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public DataTable ObtenerMecanicoFiltrados(string filtro)
        {
            try
            {
                return DatMecanico.Instancia.FiltrarMecanico(filtro);  // Llamar a la Capa de Datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public DataTable ObtenerMecanicoOrdenado2()
        {
            try
            {
                return DatMecanico.Instancia.ObtenerMecanicoOrdenado();  // Llamar a la capa de Datos
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public List<string> ObtenerCodigosEspecialidades()
        {
            try
            {
                return DatMecanico.Instancia.ObtenerCodigosEspecialidades();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de especialidades en la capa lógica: " + ex.Message);
            }
        }

        #endregion metodos
    }
}