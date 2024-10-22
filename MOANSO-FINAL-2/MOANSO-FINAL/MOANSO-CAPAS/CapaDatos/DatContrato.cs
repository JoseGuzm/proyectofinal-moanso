using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class DatContrato
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatContrato _instancia = new DatContrato();
        //privado para evitar la instanciación directa
        public static DatContrato Instancia
        {
            get
            {
                return DatContrato._instancia;
            }
        }
        #endregion singleton

        public int ObtenerNumeroBoleta()
        {
            int reg = 0;

            using (SqlConnection cnn = Conexion.Instancia.Conectar())
            {
                try
                {
                    // Crea el comando para ejecutar la consulta
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM Contratomantenimiento", cnn);
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

        public Boolean InsertarContrato(EntContrato contrato)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaContratomantenimiento", cn); 
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CodigoCM", contrato.Codigo);               // Código del contrato
                cmd.Parameters.AddWithValue("@BusCM", contrato.Bus);                     // Código del bus relacionado
                cmd.Parameters.AddWithValue("@Fecha", contrato.Fecha);                     // Fecha del contrato
                cmd.Parameters.AddWithValue("@ProveedorCM", contrato.Proveedor);         // Proveedor de mantenimiento
                cmd.Parameters.AddWithValue("@Descripcion", contrato.Descripcion);         // Descripción del mantenimiento
                cmd.Parameters.AddWithValue("@Costo", contrato.Costo);                     // Costo del mantenimiento
                cmd.Parameters.AddWithValue("@Estado", contrato.Estado);                   // Estado del contrato

                cn.Open();
                int i = cmd.ExecuteNonQuery(); // Ejecutar el procedimiento almacenado

                if (i > 0)
                {
                    inserta = true; // Si la inserción fue exitosa, se cambia el valor a true
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

            return inserta; // Retornar si la inserción fue exitosa
        }

        public Boolean DeshabilitarContrato(EntContrato contrato)
        {
            SqlCommand cmd = null;
            Boolean deshabilita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaContrato", cn); // Procedimiento almacenado para deshabilitar contrato
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro para el código del contrato
                cmd.Parameters.AddWithValue("@CodigoCM", contrato.Codigo); // Usamos el código del contrato para identificarlo

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
                string consulta = "SELECT COUNT(*) FROM Contratomantenimiento WHERE Estado = 'Activo'";  // Consulta para contar los registros

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

        public List<string> ObtenerCodigosBuses()
        {
            List<string> codigos = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT BusB FROM Bus WHERE Estado = 'Activo'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codigos.Add(reader["BusB"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de buses: " + ex.Message);
            }
            return codigos;
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


        // Método para obtener los códigos de los proveedores
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
                        cmd.Parameters.AddWithValue("@CodigoP", codigoBus);
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
    }
}