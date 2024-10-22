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
    public class DatIngresoRepuesto
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatIngresoRepuesto _instancia = new DatIngresoRepuesto();
        //privado para evitar la instanciación directa
        public static DatIngresoRepuesto Instancia
        {
            get
            {
                return DatIngresoRepuesto._instancia;
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM OrdenCompra", cnn);
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM NotaIngresoRepuesto", cnn);
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
        public Boolean InsertarIngresoRepuesto(EntIngresoRepuesto notaIngreso)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaNotaIngresoRepuestos", cn); // Use your stored procedure name here
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for NotaIngresoRepuestos
                cmd.Parameters.AddWithValue("@CodigoIR", notaIngreso.CodigoIR);
                cmd.Parameters.AddWithValue("@CodigoOC", notaIngreso.CodigoOC);
                cmd.Parameters.AddWithValue("@Fecha", notaIngreso.Fecha);
                cmd.Parameters.AddWithValue("@ProveedorIR", notaIngreso.ProveedorIR);
                cmd.Parameters.AddWithValue("@Estado", notaIngreso.Estado); // Estado can be NULL

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


        public Boolean InsertarDetalleIngresoRepuesto(EntIngresoRepuesto detalleNotaIngreso)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaDetalleNotaIngreso", cn); // Use your stored procedure name here
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for DetalleNotaIngreso
                cmd.Parameters.AddWithValue("@IRCodigo", detalleNotaIngreso.IRCodigo);
                cmd.Parameters.AddWithValue("@CantidadRecibida", detalleNotaIngreso.CantidadRecibida);
                cmd.Parameters.AddWithValue("@CodigoRepu", detalleNotaIngreso.CodigoRepu);
                cmd.Parameters.AddWithValue("@CantidadAceptada", detalleNotaIngreso.CantidadAceptada);
                cmd.Parameters.AddWithValue("@Precio", detalleNotaIngreso.Precio);

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


        public Boolean DeshabilitarIngresoRepuesto(EntIngresoRepuesto bus)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaIngresoRepuesto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoIR", bus.CodigoIR);
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

        public List<string> ObtenerCodigosProveedores(string codigoOC)
        {
            List<string> proveedores = new List<string>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string query = "SELECT DISTINCT o.CodigoPro " +
                               "FROM OrdenCompra o " +
                               "WHERE o.CodigoOC = @CodigoOC AND o.Estado = 'Activo'"; // Suponiendo que 'Activo' es un filtro de estado para proveedores
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoOC", codigoOC);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    proveedores.Add(reader["CodigoPro"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los proveedores para el Código OC: " + ex.Message);
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

        public List<RepuestoOrdenCompra> ObtenerRepuestosPorOrdenCompra(string codigoOC)
        {
            List<RepuestoOrdenCompra> repuestosOrdenCompra = new List<RepuestoOrdenCompra>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();

                // Consulta SQL para obtener los repuestos relacionados con la OrdenCompra
                string query = @"
            SELECT r.CodigoR, r.Descripcion, r.CategoriaR, r.MarcarepuestoR, dc.Cantidad
            FROM DetalleOrdenCompra dc
            INNER JOIN Repuesto r ON dc.CodigoRep = r.CodigoR
            INNER JOIN OrdenCompra o ON dc.OCCompra = o.CodigoOC
            WHERE o.CodigoOC = @CodigoOC AND r.Estado = 'Activo'";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoOC", codigoOC);
                cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Crear un objeto RepuestoOrdenCompra para almacenar los datos obtenidos
                    RepuestoOrdenCompra repuesto = new RepuestoOrdenCompra
                    {
                        CodigoRep = reader["CodigoR"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        CategoriaR = reader["CategoriaR"].ToString(),
                        MarcarepuestoR = reader["MarcarepuestoR"].ToString(),
                        Cantidad = Convert.ToInt32(reader["Cantidad"])
                    };

                    // Agregar el objeto a la lista
                    repuestosOrdenCompra.Add(repuesto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los repuestos para la Orden de Compra: " + ex.Message);
            }

            return repuestosOrdenCompra;

            #endregion metodos
        }
    }
}