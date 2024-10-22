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
    public class LogRepuesto
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogRepuesto _instancia = new LogRepuesto();
        //privado para evitar la instanciación directa
        public static LogRepuesto Instancia
        {
            get
            {
                return LogRepuesto._instancia;
            }
        }
        #endregion singleton

        #region metodos
        ///listado
        public List<EntRepuesto> ListarRepuesto2()
        {
            return DatRepuesto.Instancia.ListarRepuesto();
        }

        ///inserta
        public void InsertaRepuesto(EntRepuesto bus)
        {
            DatRepuesto.Instancia.InsertarRepuesto(bus);
        }

        public void EditaRepuesto(EntRepuesto bus)
        {
            DatRepuesto.Instancia.EditarRepuesto(bus);
        }

        public void DeshabilitarRepuesto(EntRepuesto bus)
        {
            DatRepuesto.Instancia.DeshabilitarRepuesto(bus);
        }

        public Boolean ContarTotalRepuesto(EntRepuesto bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatRepuesto.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public DataTable ObtenerRepuestoFiltrados(string filtro)
        {
            try
            {
                return DatRepuesto.Instancia.FiltrarRepuesto(filtro);  // Llamar a la Capa de Datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public DataTable ObtenerRepuestoOrdenado2()
        {
            try
            {
                return DatRepuesto.Instancia.ObtenerRepuestoOrdenado();  // Llamar a la capa de Datos
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public List<string> ObtenerCodigosProveedores()
        {
            try
            {
                return DatRepuesto.Instancia.ObtenerCodigosProveedores();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de proveedores en la capa lógica: " + ex.Message);
            }
        }

        public List<string> ObtenerCodigosMarcasRepuestos()
        {
            try
            {
                return DatRepuesto.Instancia.ObtenerCodigosMarcasRepuestos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de marcas de repuestos en la capa lógica: " + ex.Message);
            }
        }

        public List<string> ObtenerCodigosCategorias()
        {
            try
            {
                return DatRepuesto.Instancia.ObtenerCodigosCategorias();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de categorías en la capa lógica: " + ex.Message);
            }
        }

        #endregion metodos
    }
}