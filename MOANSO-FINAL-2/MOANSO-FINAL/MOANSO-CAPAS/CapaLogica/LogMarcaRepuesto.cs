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
    public class LogMarcaRepuesto
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly LogMarcaRepuesto _instancia = new LogMarcaRepuesto();
        //privado para evitar la instanciación directa
        public static LogMarcaRepuesto Instancia
        {
            get
            {
                return LogMarcaRepuesto._instancia;
            }
        }
        #endregion singleton

        #region metodos
        ///listado
        public List<EntMarcaRepuesto> ListarMarcaRepuesto2()
        {
            return DatMarcaRepuesto.Instancia.ListarMarcaRepuesto();
        }

        ///inserta
        public void InsertaMarcaRepuesto(EntMarcaRepuesto cat)
        {
            DatMarcaRepuesto.Instancia.InsertarMarcaRepuesto(cat);
        }

        public void EditaMarcaRepuesto(EntMarcaRepuesto cat)
        {
            DatMarcaRepuesto.Instancia.EditarMarcaRepuesto(cat);
        }

        public void DeshabilitarMarcaRepuesto(EntMarcaRepuesto cat)
        {
            DatMarcaRepuesto.Instancia.DeshabilitarMarcaRepuesto(cat);
        }

        public Boolean ContarTotalMarcaRepuesto(EntMarcaRepuesto bus, ref int totalRegistros)
        {
            Boolean exito = false;

            // Llamamos al método de la capa de acceso a datos para contar los registros
            exito = DatMarcaRepuesto.Instancia.ContarRegistro(ref totalRegistros);

            return exito;
        }

        public DataTable ObtenerMarcaRepuestoFiltrados(string filtro)
        {
            try
            {
                return DatMarcaRepuesto.Instancia.FiltrarMarcaRepuesto(filtro);  // Llamar a la Capa de Datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la lógica de negocio: " + ex.Message);
            }
        }

        public DataTable ObtenerMarcaRepuestoOrdenado2()
        {
            try
            {
                return DatMarcaRepuesto.Instancia.ObtenerMarcaRepuestoOrdenado();  // Llamar a la capa de Datos
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
                // Llamamos al método de la Capa de Datos utilizando la instancia Singleton
                return DatMarcaRepuesto.Instancia.ObtenerCodigosProveedores();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de proveedores en la capa lógica: " + ex.Message);
            }
        }

        #endregion metodos
    }
}