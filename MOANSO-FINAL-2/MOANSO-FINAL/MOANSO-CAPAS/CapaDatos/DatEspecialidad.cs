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
    public class DatEspecialidad
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatEspecialidad _instancia = new DatEspecialidad();
        //privado para evitar la instanciación directa
        public static DatEspecialidad Instancia
        {
            get
            {
                return DatEspecialidad._instancia;
            }
        }
        #endregion singleton

        #region metodos
        public List<EntEspecialidad> ListarEspecialidad()
        {
            SqlCommand cmd = null;
            List<EntEspecialidad> lista = new List<EntEspecialidad>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListaEspecialidad", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EntEspecialidad cat = new EntEspecialidad();
                    cat.Codigo = dr["CodigoS"].ToString();
                    cat.Nombre = dr["NombreS"].ToString();
                    cat.Descripcion = dr["Descripcion"].ToString();
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

        public Boolean InsertarEspecialidad(EntEspecialidad bus)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaEspecialidad", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoS", bus.Codigo);
                cmd.Parameters.AddWithValue("@NombreS", bus.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", bus.Descripcion);
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

        public Boolean EditarEspecialidad(EntEspecialidad bus)
        {
            SqlCommand cmd = null;
            Boolean edita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spModificarEspecialidad", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoS", bus.Codigo);
                cmd.Parameters.AddWithValue("@NombreS", bus.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", bus.Descripcion);
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

        public Boolean DeshabilitarEspecialidad(EntEspecialidad bus)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaEspecialidad", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoS", bus.Codigo);
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
                string consulta = "SELECT COUNT(*) FROM Especialidad WHERE Estado = 'Activo'";  // Consulta para contar los registros

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

        public DataTable FiltrarEspecialidad(string filtro)
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Especialidad WHERE CodigoS LIKE @Filtro OR NombreS LIKE @Filtro;";

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

        public DataTable ObtenerEspecialidadOrdenado()
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Especialidad ORDER BY NombreS ASC";

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