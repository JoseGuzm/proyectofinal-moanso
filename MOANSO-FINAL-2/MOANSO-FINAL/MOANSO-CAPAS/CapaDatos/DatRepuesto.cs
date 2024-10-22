using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class DatRepuesto
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatRepuesto _instancia = new DatRepuesto();
        //privado para evitar la instanciación directa
        public static DatRepuesto Instancia
        {
            get
            {
                return DatRepuesto._instancia;
            }
        }
        #endregion singleton

        public List<EntRepuesto> ListarRepuesto()
        {
            SqlCommand cmd = null;
            List<EntRepuesto> lista = new List<EntRepuesto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar(); //singleton
                cmd = new SqlCommand("spListaRepuesto", cn); // assuming stored procedure is named spListaRepuestos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EntRepuesto repuesto = new EntRepuesto();
                    repuesto.Codigo = dr["CodigoR"].ToString();
                    repuesto.Categoria = dr["CategoriaR"].ToString();
                    repuesto.Marcarepuesto = dr["MarcarepuestoR"].ToString();
                    repuesto.Descripcion = dr["Descripcion"].ToString();
                    repuesto.Proveedor = dr["ProveedorR"].ToString();
                    repuesto.FechaAdquisicion = Convert.ToDateTime(dr["FechaAdquisicion"]);
                    repuesto.FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]);
                    repuesto.Stock = Convert.ToInt32(dr["Stock"]);
                    repuesto.Estado = dr["Estado"].ToString();
                    lista.Add(repuesto);
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

        public Boolean InsertarRepuesto(EntRepuesto repuesto)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaRepuesto", cn); 
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CodigoR", repuesto.Codigo);
                cmd.Parameters.AddWithValue("@CategoriaR", repuesto.Categoria);
                cmd.Parameters.AddWithValue("@MarcarepuestoR", repuesto.Marcarepuesto);
                cmd.Parameters.AddWithValue("@Descripcion", repuesto.Descripcion);
                cmd.Parameters.AddWithValue("@ProveedorR", repuesto.Proveedor);
                cmd.Parameters.AddWithValue("@FechaAdquisicion", repuesto.FechaAdquisicion);
                cmd.Parameters.AddWithValue("@FechaIngreso", repuesto.FechaIngreso);
                cmd.Parameters.AddWithValue("@Stock", repuesto.Stock);
                cmd.Parameters.AddWithValue("@Estado", repuesto.Estado);

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
            finally
            {
                cmd.Connection.Close();
            }
            return inserta;
        }

        public Boolean EditarRepuesto(EntRepuesto repuesto)
        {
            SqlCommand cmd = null;
            Boolean edita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spModificarRepuesto", cn); 
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CodigoR", repuesto.Codigo);
                cmd.Parameters.AddWithValue("@CategoriaR", repuesto.Categoria);
                cmd.Parameters.AddWithValue("@MarcarepuestoR", repuesto.Marcarepuesto);
                cmd.Parameters.AddWithValue("@Descripcion", repuesto.Descripcion);
                cmd.Parameters.AddWithValue("@ProveedorR", repuesto.Proveedor);
                cmd.Parameters.AddWithValue("@FechaAdquisicion", repuesto.FechaAdquisicion);
                cmd.Parameters.AddWithValue("@FechaIngreso", repuesto.FechaIngreso);
                cmd.Parameters.AddWithValue("@Stock", repuesto.Stock);
                cmd.Parameters.AddWithValue("@Estado", repuesto.Estado);

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
            finally
            {
                cmd.Connection.Close();
            }
            return edita;
        }

        public Boolean DeshabilitarRepuesto(EntRepuesto repuesto)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaRepuesto", cn); 
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters with the Repuesto properties
                cmd.Parameters.AddWithValue("@CodigoR", repuesto.Codigo);

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
            finally
            {
                cmd.Connection.Close();
            }
            return delete;
        }

        public Boolean ContarRegistro(ref int totalRegistros)
        {
            SqlCommand cmd = null;
            Boolean exito = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string consulta = "SELECT COUNT(*) FROM Repuesto WHERE Estado = 'Activo'";  // Consulta para contar los registros

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

        public DataTable FiltrarRepuesto(string filtro)
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Repuesto WHERE CodigoR LIKE @Filtro OR Descripcion LIKE @Filtro OR CategoriaR LIKE @Filtro;";

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

        public DataTable ObtenerRepuestoOrdenado()
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Repuesto ORDER BY CodigoR ASC";

            using (SqlConnection conexion = Conexion.Instancia.Conectar())
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

        // Método para obtener los códigos de marcas de repuestos
        public List<string> ObtenerCodigosMarcasRepuestos()
        {
            List<string> codigos = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT CodigoMR FROM Marcarepuesto WHERE Estado = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codigos.Add(reader["CodigoMR"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de marcas de repuestos: " + ex.Message);
            }
            return codigos;
        }

        // Método para obtener los códigos de categorías
        public List<string> ObtenerCodigosCategorias()
        {
            List<string> codigos = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT CodigoC FROM Categoria WHERE Estado = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codigos.Add(reader["CodigoC"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de categorías: " + ex.Message);
            }
            return codigos;
        }
    }
}