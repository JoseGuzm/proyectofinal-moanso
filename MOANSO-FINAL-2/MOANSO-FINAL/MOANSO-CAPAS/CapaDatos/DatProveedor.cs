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
    public class DatProveedor
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatProveedor _instancia = new DatProveedor();
        //privado para evitar la instanciación directa
        public static DatProveedor Instancia
        {
            get
            {
                return DatProveedor._instancia;
            }
        }
        #endregion singleton


        #region metodos
        public List<EntProveedor> ListarProveedor()
        {
            SqlCommand cmd = null;
            List<EntProveedor> lista = new List<EntProveedor>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar(); //singleton
                cmd = new SqlCommand("spListaProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EntProveedor bus = new EntProveedor();
                    bus.Codigo = dr["CodigoP"].ToString();
                    bus.Nombre = dr["NombreEmpresa"].ToString();
                    bus.Razon = dr["RazonSocial"].ToString();
                    bus.Tipo = dr["Tipo"].ToString();
                    bus.RUC = dr["RUC"].ToString();
                    bus.Direccion = dr["Direccion"].ToString();
                    bus.Telefono = dr["Telefono"].ToString();
                    bus.Estado = dr["Estado"].ToString();
                    lista.Add(bus);
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

        public Boolean InsertarProveedor(EntProveedor bus)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoP", bus.Codigo);
                cmd.Parameters.AddWithValue("@NombreEmpresa", bus.Nombre);
                cmd.Parameters.AddWithValue("@RazonSocial", bus.Razon);
                cmd.Parameters.AddWithValue("@Tipo", bus.Tipo);
                cmd.Parameters.AddWithValue("@RUC", bus.RUC);
                cmd.Parameters.AddWithValue("@Direccion", bus.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", bus.Telefono);
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

        public Boolean EditarProveedor(EntProveedor bus)
        {
            SqlCommand cmd = null;
            Boolean edita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spModificarProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoP", bus.Codigo);
                cmd.Parameters.AddWithValue("@NombreEmpresa", bus.Nombre);
                cmd.Parameters.AddWithValue("@RazonSocial", bus.Razon);
                cmd.Parameters.AddWithValue("@Tipo", bus.Tipo);
                cmd.Parameters.AddWithValue("@RUC", bus.RUC);
                cmd.Parameters.AddWithValue("@Direccion", bus.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", bus.Telefono);
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

        public Boolean DeshabilitarProveedor(EntProveedor bus)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoP", bus.Codigo);
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
                string consulta = "SELECT COUNT(*) FROM Proveedor WHERE Estado = 'Activo'";  // Consulta para contar los registros

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

        public DataTable FiltrarProveedor(string filtro)
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Proveedor WHERE CodigoP LIKE @Filtro OR NombreEmpresa LIKE @Filtro OR RUC LIKE @Filtro;";

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

        public DataTable ObtenerProveedorOrdenado()
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Proveedor ORDER BY NombreEmpresa ASC";

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
                        throw new Exception("Error al obtener los Proveedor ordenados: " + ex.Message);
                    }
                }
            }

            return dtbus;
        }

        #endregion metodos
    }
}