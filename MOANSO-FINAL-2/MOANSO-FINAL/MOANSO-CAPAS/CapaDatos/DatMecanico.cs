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
    public class DatMecanico
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatMecanico _instancia = new DatMecanico();
        //privado para evitar la instanciación directa
        public static DatMecanico Instancia
        {
            get
            {
                return DatMecanico._instancia;
            }
        }
        #endregion singleton


        #region metodos
        public List<EntMecanico> ListarMecanico()
        {
            SqlCommand cmd = null;
            List<EntMecanico> lista = new List<EntMecanico>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar(); //singleton
                cmd = new SqlCommand("spListaMecanico", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EntMecanico bus = new EntMecanico();
                    bus.Codigo = dr["CodigoM"].ToString();
                    bus.Especialidad = dr["Especialidad"].ToString();
                    bus.Nombre = dr["Nombre"].ToString();
                    bus.DNI = dr["DNI"].ToString();
                    bus.Domicilio = dr["Domicilio"].ToString();
                    bus.Experiencia = dr["Experiencia"].ToString();
                    bus.Telefono = dr["Telefono"].ToString();
                    bus.Sueldo = Convert.ToDouble(dr["Sueldo"]);
                    bus.Turno = dr["Turno"].ToString();
                    bus.FechaContrato = Convert.ToDateTime(dr["FechaContrato"]);
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

        public Boolean InsertarMecanico(EntMecanico bus)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaMecanico", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoM", bus.Codigo);
                cmd.Parameters.AddWithValue("@Especialidad", bus.Especialidad);
                cmd.Parameters.AddWithValue("@Nombre", bus.Nombre);
                cmd.Parameters.AddWithValue("@DNI", bus.DNI);
                cmd.Parameters.AddWithValue("@Domicilio", bus.Domicilio);
                cmd.Parameters.AddWithValue("@Especialidad", bus.Especialidad);
                cmd.Parameters.AddWithValue("@Telefono", bus.Telefono);
                cmd.Parameters.AddWithValue("@Sueldo", bus.Sueldo);
                cmd.Parameters.AddWithValue("@Turno", bus.Turno);
                cmd.Parameters.AddWithValue("@FechaContrato", bus.FechaContrato);
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

        public Boolean EditarMecanico(EntMecanico bus)
        {
            SqlCommand cmd = null;
            Boolean edita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spModificarBus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoM", bus.Codigo);
                cmd.Parameters.AddWithValue("@Especialidad", bus.Especialidad);
                cmd.Parameters.AddWithValue("@Nombre", bus.Nombre);
                cmd.Parameters.AddWithValue("@DNI", bus.DNI);
                cmd.Parameters.AddWithValue("@Domicilio", bus.Domicilio);
                cmd.Parameters.AddWithValue("@Especialidad", bus.Especialidad);
                cmd.Parameters.AddWithValue("@Telefono", bus.Telefono);
                cmd.Parameters.AddWithValue("@Sueldo", bus.Sueldo);
                cmd.Parameters.AddWithValue("@Turno", bus.Turno);
                cmd.Parameters.AddWithValue("@FechaContrato", bus.FechaContrato);
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

        public Boolean DeshabilitarMecanico(EntMecanico bus)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaMecanico", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoM", bus.Codigo);
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
                string consulta = "SELECT COUNT(*) FROM Mecanico WHERE Estado = 'Activo'";  // Consulta para contar los registros

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

        public DataTable FiltrarMecanico(string filtro)
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Mecanico WHERE CodigoM LIKE @Filtro OR Nombre LIKE @Filtro OR DNI LIKE @Filtro;";

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

        public DataTable ObtenerMecanicoOrdenado()
        {
            DataTable dtbus = new DataTable();
            string sql = "SELECT * FROM Mecanico ORDER BY Nombre ASC";

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

        public List<string> ObtenerCodigosEspecialidades()
        {
            List<string> codigos = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT CodigoS FROM Especialidad WHERE Estado = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codigos.Add(reader["CodigoS"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de especialidades: " + ex.Message);
            }
            return codigos;
        }

        #endregion metodos
    }
}