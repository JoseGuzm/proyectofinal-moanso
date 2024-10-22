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
    public class DatEvaluacionExterna
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatEvaluacionExterna _instancia = new DatEvaluacionExterna();
        //privado para evitar la instanciación directa
        public static DatEvaluacionExterna Instancia
        {
            get
            {
                return DatEvaluacionExterna._instancia;
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM OrdenTrabajoExterno", cnn);
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM EvalucionExterna", cnn);
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

        public Boolean InsertarEvaluacionExterna(EntEvaluacionExterna evaluacion)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                // Establish a connection to the database
                SqlConnection cn = Conexion.Instancia.Conectar();

                // Define the stored procedure for inserting the data
                cmd = new SqlCommand("spInsertaEvaluacionExterna", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters with values from the EvaluacionExterna object
                cmd.Parameters.AddWithValue("@CodigoEE", evaluacion.CodigoEE);               // Código de la Evaluación Externa
                cmd.Parameters.AddWithValue("@CodigoBus", evaluacion.CodigoBus);             // Código del Bus relacionado
                cmd.Parameters.AddWithValue("@ProveedorEE", evaluacion.ProveedorEE);         // Proveedor de la evaluación externa
                cmd.Parameters.AddWithValue("@Fecha", evaluacion.Fecha);                     // Fecha de la evaluación
                cmd.Parameters.AddWithValue("@TECodigo", evaluacion.TECodigo);               // Código del técnico responsable
                cmd.Parameters.AddWithValue("@Estado", evaluacion.Estado ?? (object)DBNull.Value);  // Estado de la evaluación (puede ser null)

                // Open the connection
                cn.Open();

                // Execute the stored procedure
                int i = cmd.ExecuteNonQuery(); // Ejecutar el procedimiento almacenado

                if (i > 0)
                {
                    inserta = true; // If the insertion is successful, set inserta to true
                }
            }
            catch (Exception e)
            {
                throw e; // Propagate the exception if any error occurs
            }
            finally
            {
                cmd.Connection.Close(); // Close the connection
            }

            return inserta; // Return true if insertion was successful, otherwise false
        }


        public Boolean DeshabilitarEvaluacionExterna(EntEvaluacionExterna evaluacion)
        {
            SqlCommand cmd = null;
            Boolean deshabilita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaEvaluacionExterna", cn); // Procedimiento almacenado para deshabilitar evaluación externa
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregar parámetros para los datos de la evaluación externa
                cmd.Parameters.AddWithValue("@CodigoEE", evaluacion.CodigoEE); // Código de la evaluación externa
                //cmd.Parameters.AddWithValue("@Estado", "Deshabilitado"); // Estado para deshabilitar la evaluación externa

                cn.Open();
                int i = cmd.ExecuteNonQuery(); // Ejecutar el procedimiento almacenado

                if (i > 0)
                {
                    deshabilita = true; // Si la operación fue exitosa, devolver true
                }
            }
            catch (Exception e)
            {
                throw e; // Propagar la excepción en caso de error
            }
            finally
            {
                cmd.Connection.Close(); // Cerrar la conexión
            }

            return deshabilita; // Retornar si la deshabilitación fue exitosa
        }


        public Boolean ContarRegistro(ref int totalRegistros)
        {
            SqlCommand cmd = null;
            Boolean exito = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string consulta = "SELECT COUNT(*) FROM EvaluacionExterna WHERE Estado = 'Activo'";  // Consulta para contar los registros

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

        public List<string> ObtenerCodigosBuses(string codigoTE)
        {
            List<string> buses = new List<string>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                // Consultamos los buses asociados con el CodigoTE
                string query = "SELECT b.BusB " +
                               "FROM Bus b " +
                               "INNER JOIN OrdenTrabajoExterno ote ON ote.CodigoBus = b.BusB " +
                               "WHERE ote.CodigoTE = @CodigoTE AND b.Estado = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoTE", codigoTE);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    buses.Add(reader["BusB"].ToString()); // Agregar los códigos de buses al listado
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los buses para el Código TE: " + ex.Message);
            }

            return buses;
        }

        // Método para obtener los códigos de los proveedores
        public List<string> ObtenerCodigosProveedores(string codigoTE)
        {
            List<string> proveedores = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                // Consulta SQL para obtener los proveedores asociados al CodigoTE
                string query = "SELECT p.CodigoP FROM Proveedor p " +
                               "INNER JOIN OrdenTrabajoExterno ote ON ote.ProveedorTE = p.CodigoP " +
                               "WHERE ote.CodigoTE = @CodigoTE AND p.Estado = 'Activo'"; // Filtramos por CodigoTE y estado activo

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoTE", codigoTE); // Usamos el parámetro para evitar SQL Injection
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    proveedores.Add(reader["CodigoP"].ToString()); // Agregamos los proveedores al listado
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los proveedores por CodigoTE: " + ex.Message);
            }

            return proveedores;
        }

        public EntProveedor ObtenerDatosProve(string codigoBus)
        {
            EntProveedor bus = null;
            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    string query = "SELECT NombreEmpresa, RUC, RazonSocial, Tipo, Telefono FROM Proveedor WHERE CodigoP = @CodigoP AND Estado = 'Activo'";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@codigoBus", codigoBus);
                        cn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bus = new EntProveedor
                                {
                                    Nombre = reader["NombreEmpresa"].ToString(),
                                    RUC = reader["RUC"].ToString(),
                                    Razon = reader["RazonSocial"].ToString(),
                                    Tipo = reader["Tipo"].ToString(),
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
    }
}