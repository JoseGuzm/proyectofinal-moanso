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
    public class DatOrdenCompra
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatOrdenCompra _instancia = new DatOrdenCompra();
        //privado para evitar la instanciación directa
        public static DatOrdenCompra Instancia
        {
            get
            {
                return DatOrdenCompra._instancia;
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM OrdenPedido", cnn);
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM OrdenCompra", cnn);
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

        public Boolean InsertarOrdenCompra(EntOrdenCompra ordenCompra)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaOrdenCompra", cn); // Stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // Parameters for inserting into the OrdenCompra table
                cmd.Parameters.AddWithValue("@CodigoOC", ordenCompra.CodigoOC);          // Order Code
                cmd.Parameters.AddWithValue("@CodigoPro", ordenCompra.CodigoPro);        // Product Code
                cmd.Parameters.AddWithValue("@Fecha", ordenCompra.Fecha);                // Date of Order
                cmd.Parameters.AddWithValue("@OPCodigo", ordenCompra.OPCodigo);          // Order Payment Code
                cmd.Parameters.AddWithValue("@FormaPago", ordenCompra.FormaPago);        // Payment Method
                cmd.Parameters.AddWithValue("@TOTAL", ordenCompra.Total);                // Total Amount
                cmd.Parameters.AddWithValue("@Estado", ordenCompra.Estado);              // Order Status

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


        public Boolean InsertarDetalleOrdenCompra(EntOrdenCompra detalleOrdenCompra)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaDetalleOrdenCompra", cn); // Stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // Parameters for inserting into the DetalleOrdenCompra table
                cmd.Parameters.AddWithValue("@OCCompra", detalleOrdenCompra.OCCompra);   // Order Code reference
                cmd.Parameters.AddWithValue("@Cantidad", detalleOrdenCompra.Cantidad);   // Quantity of items
                cmd.Parameters.AddWithValue("@CodigoRep", detalleOrdenCompra.CodigoRep); // Replacement part/product code
                cmd.Parameters.AddWithValue("@Precio", detalleOrdenCompra.Precio);       // Price of the product/item

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


        public Boolean InsertarFactura(EntFactura ordenCompra)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaFactura", cn); // Stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // Parameters for inserting into the OrdenCompra table
                cmd.Parameters.AddWithValue("@CodigoFactura", ordenCompra.CodigoFactura);          // Order Code
                cmd.Parameters.AddWithValue("@CodigoOC", ordenCompra.CodigoOC);        // Product Code
                cmd.Parameters.AddWithValue("@Fecha", ordenCompra.Fecha);                // Date of Order        // Payment Method
                cmd.Parameters.AddWithValue("@TOTAL", ordenCompra.TOTAL);                // Total Amount              // Order Status

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


        public Boolean DeshabilitarOrdenCompra(EntOrdenCompra evaluacion)
        {
            SqlCommand cmd = null;
            Boolean deshabilita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaEvaluacionInterna", cn); // Procedimiento almacenado
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros para deshabilitar la evaluación interna
                cmd.Parameters.AddWithValue("@CodigoOC", evaluacion.CodigoOC); // Codigo de la Evaluación Interna
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

        public List<string> ObtenerProveedores()
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
