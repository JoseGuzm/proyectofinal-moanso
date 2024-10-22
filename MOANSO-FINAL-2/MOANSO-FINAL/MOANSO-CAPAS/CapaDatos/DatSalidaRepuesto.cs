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
    public class DatSalidaRepuesto
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatSalidaRepuesto _instancia = new DatSalidaRepuesto();
        //privado para evitar la instanciación directa
        public static DatSalidaRepuesto Instancia
        {
            get
            {
                return DatSalidaRepuesto._instancia;
            }
        }
        #endregion singleton


        #region metodos

        public int ObtenerNumeroBoleta2()
        {
            int reg = 0;

            using (SqlConnection cnn = Conexion.Instancia.Conectar())
            {
                try
                {
                    // Crea el comando para ejecutar la consulta
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM OrdenPedido", cnn);
                    cnn.Open();
                    reg = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM NotaSalidaRepuesto", cnn);
                    cnn.Open();
                    reg = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return reg;
        }
        public Boolean InsertarSalidaRepuesto(EntSalidaRepuesto notaSalidaRepuesto)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaSalidaRepuesto", cn); // Stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for NotaSalidaRepuesto
                cmd.Parameters.AddWithValue("@CodigoSR", notaSalidaRepuesto.CodigoSR);  // CodigoSR
                cmd.Parameters.AddWithValue("@BusSR", notaSalidaRepuesto.BusSR);        // BusSR
                cmd.Parameters.AddWithValue("@Fecha", notaSalidaRepuesto.Fecha);        // Fecha
                cmd.Parameters.AddWithValue("@OPCodigo", notaSalidaRepuesto.OPCodigo);  // OPCodigo
                cmd.Parameters.AddWithValue("@Estado", notaSalidaRepuesto.Estado);      // Estado (nullable)

                cn.Open();
                int i = cmd.ExecuteNonQuery();  // Execute the command
                if (i > 0)
                {
                    inserta = true;  // If one row was affected, return true
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




        public Boolean InsertarDetalleSalidaRepuesto(EntSalidaRepuesto detalleNotaSalida)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaDetalleSalidaRepuesto", cn); // Stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for DetalleNotaSalida
                cmd.Parameters.AddWithValue("@DSRCodigo", detalleNotaSalida.DSRCodigo);  // DSRCodigo (reference to NotaSalidaRepuesto)
                cmd.Parameters.AddWithValue("@CantidadRecibida", detalleNotaSalida.CantidadRecibida);  // CantidadRecibida
                cmd.Parameters.AddWithValue("@CodigoRepu", detalleNotaSalida.CodigoRepu);  // CodigoRepu (spare part code)
                cmd.Parameters.AddWithValue("@CantidadEnviada", detalleNotaSalida.CantidadEnviada);  // CantidadEnviada

                cn.Open();
                int i = cmd.ExecuteNonQuery();  // Execute the command
                if (i > 0)
                {
                    inserta = true;  // If one row was affected, return true
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



        public Boolean DeshabilitarSalidaRepuesto(EntSalidaRepuesto bus)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaSalidaRepuesto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoSR", bus.CodigoSR);
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

        public List<string> ObtenerCodigosBus(string codigoOP)
        {
            List<string> buses = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT DISTINCT np.BusSR " +
                               "FROM NotaSalidaRepuesto np " +
                               "JOIN OrdenPedido op ON np.OPCodigo = op.CodigoOP " +  // Cambié CodigoOP por OPCodigo
                               "WHERE op.CodigoOP = @CodigoOP AND np.Estado = 'Activo'"; // Filtrando por estado 'Activo'
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoOP", codigoOP);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    buses.Add(reader["BusSR"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los códigos de buses para el Código OP: " + ex.Message);
            }

            return buses;
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

        #endregion metodos
    }
}
