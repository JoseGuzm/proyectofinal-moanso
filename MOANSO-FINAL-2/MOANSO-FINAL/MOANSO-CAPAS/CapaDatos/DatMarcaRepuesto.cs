using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DatMarcaRepuesto
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatMarcaRepuesto _instancia = new DatMarcaRepuesto();
        //privado para evitar la instanciación directa
        public static DatMarcaRepuesto Instancia
        {
            get
            {
                return DatMarcaRepuesto._instancia;
            }
        }
        #endregion singleton

        public List<string> ObtenerCodigosProveedores()
        {
            List<string> codigos = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT CodigoP FROM Proveedor WHERE Estado = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codigos.Add(reader["CodigoP"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de proveedores: " + ex.Message);
            }
            return codigos;
        }


        #region metodos
        public List<EntMarcaRepuesto> ListarMarcaRepuesto()
        {
            SqlCommand cmd = null;
            List<EntMarcaRepuesto> lista = new List<EntMarcaRepuesto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListaMarcarepuesto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EntMarcaRepuesto cat = new EntMarcaRepuesto();
                    cat.Codigo = dr["CodigoMR"].ToString();
                    cat.Descripcion = dr["Descripcion"].ToString();
                    cat.Proveedor = dr["ProveedorMR"].ToString();
                    cat.Estado = dr["Estado"].ToString();
                    lista.Add(cat);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        public Boolean InsertarMarcaRepuesto(EntMarcaRepuesto bus)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaMarcaRepuesto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoS", bus.Codigo);
                cmd.Parameters.AddWithValue("@Descripcion", bus.Descripcion);
                cmd.Parameters.AddWithValue("@ProveedorMR", bus.Proveedor);
                cmd.Parameters.AddWithValue("@Estado", bus.Estado);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    inserta = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return inserta;
        }

        public Boolean EditarMarcaRepuesto(EntMarcaRepuesto bus)
        {
            SqlCommand cmd = null;
            Boolean edita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spModificarMarcaRepuesto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoMR", bus.Codigo);
                cmd.Parameters.AddWithValue("@Descripcion", bus.Descripcion);
                cmd.Parameters.AddWithValue("@ProveedorMR", bus.Proveedor);
                cmd.Parameters.AddWithValue("@Estado", bus.Estado);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    edita = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return edita;
        }

        public Boolean DeshabilitarMarcaRepuesto(EntMarcaRepuesto bus)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaMarcaRepuesto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoMR", bus.Codigo);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    delete = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return delete;
        }

        public Boolean ContarRegistro(ref int totalRegistros)
        {
            SqlCommand cmd = null;
            Boolean exito = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string consulta = "SELECT COUNT(*) FROM Marcarepuesto WHERE Estado = 'Activo'";  // Consulta para contar los registros

                cmd = new SqlCommand(consulta, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();

                totalRegistros = (int)cmd.ExecuteScalar();  // Ejecutamos la consulta y obtenemos el valor de COUNT

                // Si hay registros, asignamos true a exito
                if (totalRegistros > 0)
                {
                    exito = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
            return exito;
        }

        public DataTable FiltrarMarcaRepuesto(string filtro)
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Marcarepuesto WHERE CodigoMR LIKE @Filtro OR ProveedorMR LIKE @Filtro;";

            using (SqlConnection conexion = Conexion.Instancia.Conectar())  // Uso de la instancia de Conexion
            {
                if (conexion != null)
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(sql, conexion);
                        command.Parameters.AddWithValue("@Filtro", "%" + filtro + "%");

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(dtbus);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudieron mostrar los registros, error: " + ex.Message);
                    }
                }
            }
            return dtbus;  // Retorna el DataTable con los registros filtrados
        }

        public DataTable ObtenerMarcaRepuestoOrdenado()
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Marcarepuesto ORDER BY Descripcion ASC";

            using (SqlConnection conexion = Conexion.Instancia.Conectar())  // Asumiendo que la conexión está gestionada por la clase Conexion
            {
                if (conexion != null)
                {
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conexion);
                        dataAdapter.Fill(dtbus);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al obtener los clientes ordenados: " + ex.Message);
                    }
                }
            }

            return dtbus;
        }

        #endregion metodos
    }
}