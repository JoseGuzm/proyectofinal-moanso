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
    public class DatOTI
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatOTI _instancia = new DatOTI();
        //privado para evitar la instanciación directa
        public static DatOTI Instancia
        {
            get
            {
                return DatOTI._instancia;
            }
        }
        #endregion singleton

        /*
        public int ObtenerNumeroBoleta2()
        {
            int reg = 0;

            using (SqlConnection cnn = Conexion.Instancia.Conectar())
            {
                try
                {
                    // Crea el comando para ejecutar la consulta
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM OrdenTrabajoInterno", cnn);
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
        */

        public int ObtenerNumeroBoleta()
        {
            int reg = 0;

            using (SqlConnection cnn = Conexion.Instancia.Conectar())
            {
                try
                {
                    // Crea el comando para ejecutar la consulta
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM OrdenTrabajoInterno", cnn);
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

        public Boolean InsertarOTI(EntOTI ordenTrabajoInterno)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaOTI", cn); // Stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for OrdenTrabajoInterno
                cmd.Parameters.AddWithValue("@CodigoTI", ordenTrabajoInterno.CodigoTI);
                cmd.Parameters.AddWithValue("@BusTI", ordenTrabajoInterno.BusTI);
                cmd.Parameters.AddWithValue("@Fecha", ordenTrabajoInterno.Fecha);
                cmd.Parameters.AddWithValue("@Estado", ordenTrabajoInterno.Estado);

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


        public Boolean InsertarDetalleOTI(EntOTI detalleOTI)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaDetalleOTI", cn); // Stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for DetalleOTI
                cmd.Parameters.AddWithValue("@OrdentrabajointernoID", detalleOTI.OrdentrabajointernoID);
                cmd.Parameters.AddWithValue("@CodigoRepu", detalleOTI.CodigoRepu);
                cmd.Parameters.AddWithValue("@MecanicoTI", detalleOTI.MecanicoTI);
                cmd.Parameters.AddWithValue("@Parte", detalleOTI.Parte);
                cmd.Parameters.AddWithValue("@Pieza", detalleOTI.Pieza);
                cmd.Parameters.AddWithValue("@Cantidad", detalleOTI.Cantidad);

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


        public Boolean DeshabilitarOTI(EntOTI evaluacion)
        {
            SqlCommand cmd = null;
            Boolean deshabilita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spOTI", cn); // Procedimiento almacenado
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros para deshabilitar la evaluación interna
                cmd.Parameters.AddWithValue("@CodigoTI", evaluacion.CodigoTI);     

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

        public List<string> ObtenerCodigosBus()
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

        public List<string> ObtenerCodigosMecanico()
        {
            List<string> codigosMecanico = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT CodigoM FROM Mecanico WHERE Estado = 'Activo'"; // Obtenemos los códigos de los mecánicos de la tabla DetalleOTI
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codigosMecanico.Add(reader["CodigoM"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de los mecánicos: " + ex.Message);
            }
            return codigosMecanico;
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