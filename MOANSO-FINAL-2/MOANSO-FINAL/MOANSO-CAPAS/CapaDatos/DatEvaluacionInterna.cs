using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using CapaEntidad;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DatEvaluacionInterna
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatEvaluacionInterna _instancia = new DatEvaluacionInterna();
        //privado para evitar la instanciación directa
        public static DatEvaluacionInterna Instancia
        {
            get
            {
                return DatEvaluacionInterna._instancia;
            }
        }
        #endregion singleton

        public int ObtenerNumeroBoleta2()
        {
            int reg = 0;

            using (SqlConnection cnn = Conexion.Instancia.Conectar())
            {
                try
                {
                    // Crea el comando para ejecutar la consulta
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from OrdenTrabajoInterno", cnn);
                    cnn.Open();
                    reg = Convert.ToInt32(cmd.ExecuteScalar()); // Ejecuta la consulta y obtiene el resultado como int
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // En caso de error
                }
            }

            return reg;
        }

        public int ObtenerNumeroBoleta()
        {
            int reg = 0;

            using (SqlConnection cnn = Conexion.Instancia.Conectar())
            {
                try
                {
                    // Crea el comando para ejecutar la consulta
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM EvaluacionInterna", cnn);
                    cnn.Open();
                    reg = Convert.ToInt32(cmd.ExecuteScalar()); // Ejecuta la consulta y obtiene el resultado como int
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // En caso de error
                }
            }

            return reg;
        }

        public Boolean InsertarEvaluacionInterna(EntEvaluacionInterna evaluacion)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaEvaluacionInterna", cn); // Procedimiento almacenado
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros para insertar en la tabla EvaluacionInterna
                cmd.Parameters.AddWithValue("@CodigoEI", evaluacion.CodigoEI);
                cmd.Parameters.AddWithValue("@CodigoBus", evaluacion.CodigoBus);
                cmd.Parameters.AddWithValue("@Fecha", evaluacion.Fecha);
                cmd.Parameters.AddWithValue("@TICodigo", evaluacion.TICodigo);
                cmd.Parameters.AddWithValue("@Estado", evaluacion.Estado);

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

        public Boolean InsertarDetalleEvaluacionInterna(EntEvaluacionInterna detalle)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaDetalleEvaluacionInterna", cn); // Procedimiento almacenado
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EICodigo", detalle.EICodigo); // Código de la evaluación interna
                cmd.Parameters.AddWithValue("@MecanicoEI", detalle.MecanicoEI); // Código del mecánico

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

        public Boolean DeshabilitarEvaluacionInterna(EntEvaluacionInterna evaluacion)
        {
            SqlCommand cmd = null;
            Boolean deshabilita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaEvaluacionInterna", cn); // Procedimiento almacenado
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros para deshabilitar la evaluación interna
                cmd.Parameters.AddWithValue("@CodigoEI", evaluacion.CodigoEI); // Codigo de la Evaluación Interna
                //cmd.Parameters.AddWithValue("@Estado", "Deshabilitado");        // Estado de la evaluación, podría ser "Deshabilitado"

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    deshabilita = true;
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
            return deshabilita;
        }

        public Boolean ContarRegistro(ref int totalRegistros)
        {
            SqlCommand cmd = null;
            Boolean exito = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string consulta = "SELECT COUNT(*) FROM EvaluacionInterna WHERE Estado = 'Activo'";  // Consulta para contar los registros

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

        public List<string> ObtenerCodigosBuses(string codigoTI)
        {
            List<string> buses = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT b.BusB FROM Bus b " +
                               "JOIN OrdenTrabajoInterno oti ON b.BusB = oti.BusTI " +
                               "WHERE oti.CodigoTI = @CodigoTI AND b.Estado = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoTI", codigoTI);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    buses.Add(reader["BusB"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los buses para el código TI: " + ex.Message);
            }

            return buses;
        }

        // Método para obtener los códigos de los mecánicos
        public List<string> ObtenerCodigosMecanicos(string codigoTI)
        {
            List<string> mecanicos = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                // Modificamos la consulta para obtener más detalles del mecánico
                string query = "SELECT m.CodigoM, m.Nombre, m.DNI, m.Domicilio, m.Experiencia, m.Telefono, m.Sueldo, m.Turno, m.FechaContrato " +
                               "FROM Mecanico m " +
                               "JOIN DetalleOTI doti ON m.CodigoM = doti.MecanicoTI " +
                               "JOIN OrdenTrabajoInterno oti ON doti.OrdentrabajointernoID = oti.CodigoTI " +
                               "WHERE oti.CodigoTI = @CodigoTI AND m.Estado = 'Activo'";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoTI", codigoTI);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                // Añadimos los detalles de cada mecánico en el ComboBox
                while (reader.Read())
                {
                    string mecanicoInfo = $"{reader["CodigoM"].ToString()} - {reader["Nombre"].ToString()}";
                    mecanicos.Add(mecanicoInfo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los mecánicos para el código TI: " + ex.Message);
            }

            return mecanicos;
        }

        public EntBus ObtenerDatosBus(string codigoBus)
        {
            EntBus bus = null;
            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    string query = "SELECT Marca, Modelo, NPlaca, NChasis, Combustible, Capacidad FROM Bus WHERE BusB = @codigoBus AND Estado = 'Activo'";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@codigoBus", codigoBus);
                        cn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bus = new EntBus
                                {
                                    Marca = reader["Marca"].ToString(),
                                    Modelo = reader["Modelo"].ToString(),
                                    NPlaca = reader["NPlaca"].ToString(),
                                    NChasis = reader["NChasis"].ToString(),
                                    Combustible = reader["Combustible"].ToString(),
                                    Capacidad = Convert.ToInt32(reader["Capacidad"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus: " + ex.Message);
            }

            return bus;
        }

        public EntMecanico ObtenerDatosMeca(string codigoBus)
        {
            EntMecanico bus = null;
            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    string query = "SELECT Nombre, Especialidad, DNI, Telefono FROM Mecanico WHERE CodigoM = @CodigoM AND Estado = 'Activo'";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@codigoBus", codigoBus);
                        cn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bus = new EntMecanico
                                {
                                    Nombre = reader["Nombre"].ToString(),
                                    Especialidad = reader["EspecialidadM"].ToString(),
                                    DNI = reader["DNI"].ToString(),
                                    Telefono = reader["Telefono"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos del bus: " + ex.Message);
            }

            return bus;
        }
    }
}
