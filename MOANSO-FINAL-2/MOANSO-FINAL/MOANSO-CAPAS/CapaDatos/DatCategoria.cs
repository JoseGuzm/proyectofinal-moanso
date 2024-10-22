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
    public class DatCategoria
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatCategoria _instancia = new DatCategoria();
        //privado para evitar la instanciación directa
        public static DatCategoria Instancia
        {
            get
            {
                return DatCategoria._instancia;
            }
        }
        #endregion singleton

        #region metodos
        public List<EntCategoria> ListarCategoria()
        {
            SqlCommand cmd = null;
            List<EntCategoria> lista = new List<EntCategoria>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListaCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EntCategoria cat = new EntCategoria();
                    cat.Codigo = dr["CodigoC"].ToString();
                    cat.Nombre = dr["NombreC"].ToString();
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

        public Boolean InsertarCategoria(EntCategoria bus)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoC", bus.Codigo);
                cmd.Parameters.AddWithValue("@NombreC", bus.Nombre);
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

        public Boolean EditarCategoria(EntCategoria bus)
        {
            SqlCommand cmd = null;
            Boolean edita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spModificarCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoriaC", bus.Codigo);
                cmd.Parameters.AddWithValue("@NombreC", bus.Nombre);
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

        public Boolean DeshabilitarCategoria(EntCategoria bus)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoC", bus.Codigo);
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
                string consulta = "SELECT COUNT(*) FROM Categoria WHERE Estado = 'Activo'";  // Consulta para contar los registros

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

        public DataTable FiltrarCategoria(string filtro)
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Categoria WHERE CodigoC LIKE @Filtro OR NombreC LIKE @Filtro;";

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

        public DataTable ObtenerCategoriaOrdenado()
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Categoria ORDER BY NombreC ASC";

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