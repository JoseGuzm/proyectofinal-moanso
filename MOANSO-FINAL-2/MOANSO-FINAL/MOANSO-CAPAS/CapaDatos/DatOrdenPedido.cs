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
    public class DatOrdenPedido
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatOrdenPedido _instancia = new DatOrdenPedido();
        //privado para evitar la instanciación directa
        public static DatOrdenPedido Instancia
        {
            get
            {
                return DatOrdenPedido._instancia;
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM OrdenTrabajoInterno", cnn);
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM OrdenPedido", cnn);
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
        public Boolean InsertarOrdenPedido(EntOrdenPedido ordenPedido)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaOrdenPedido", cn); // Stored procedure for OrdenPedido
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for OrdenPedido
                cmd.Parameters.AddWithValue("@CodigoOP", ordenPedido.CodigoOP);        // Order Code
                cmd.Parameters.AddWithValue("@Fecha", ordenPedido.Fecha);              // Order Date
                cmd.Parameters.AddWithValue("@TICodigo", ordenPedido.TICodigo);        // Ticket Code
                cmd.Parameters.AddWithValue("@JefeEncargado", ordenPedido.JefeEncargado); // Person in charge
                cmd.Parameters.AddWithValue("@Descripcion", ordenPedido.Descripcion);   // Description of the order
                cmd.Parameters.AddWithValue("@Estado", ordenPedido.Estado);            // Order Status

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



        public Boolean InsertarDetalleOrdenPedido(EntOrdenPedido detalleOrdenPedido)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaDetalleOrdenPedido", cn); // Stored procedure for DetalleOrdenPedido
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for DetalleOrdenPedido
                cmd.Parameters.AddWithValue("@OPCodigo", detalleOrdenPedido.OPCodigo);   // Order Code reference
                cmd.Parameters.AddWithValue("@Cantidad", detalleOrdenPedido.Cantidad);   // Quantity of items ordered
                cmd.Parameters.AddWithValue("@CodigoRepu", detalleOrdenPedido.CodigoRepu); // Product/Replacement part code

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



        public Boolean DeshabilitarOrdenPedido(EntOrdenPedido bus)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaOrdenPedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodigoOP", bus.CodigoOP);
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

        #endregion metodos
    }
}
