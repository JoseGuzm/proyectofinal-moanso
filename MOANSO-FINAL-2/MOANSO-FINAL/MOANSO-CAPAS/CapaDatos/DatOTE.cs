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
    public class DatOTE
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatOTE _instancia = new DatOTE();
        //privado para evitar la instanciación directa
        public static DatOTE Instancia
        {
            get
            {
                return DatOTE._instancia;
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM Contratomantenimiento", cnn);
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
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) + 1 FROM OrdenTrabajoExterno", cnn);
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

        public Boolean InsertarOTE(EntOTE ordenTrabajoExterno)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaOrdenTrabajoExterno", cn); // Stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for OrdenTrabajoExterno
                cmd.Parameters.AddWithValue("@CodigoTE", ordenTrabajoExterno.CodigoTE);
                cmd.Parameters.AddWithValue("@CodigoBus", ordenTrabajoExterno.CodigoBus);
                cmd.Parameters.AddWithValue("@ContratoCO", ordenTrabajoExterno.ContratoCO);
                cmd.Parameters.AddWithValue("@Fecha", ordenTrabajoExterno.Fecha);
                cmd.Parameters.AddWithValue("@ProveedorTE", ordenTrabajoExterno.ProveedorTE);
                cmd.Parameters.AddWithValue("@Estado", ordenTrabajoExterno.Estado);

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


        public Boolean InsertarDetalleOTE(EntOTE detalleOTE)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertaDetalleOTE", cn); // Stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding parameters for DetalleOTE
                cmd.Parameters.AddWithValue("@TECodigo", detalleOTE.TECodigo);
                cmd.Parameters.AddWithValue("@CodigoRepu", detalleOTE.CodigoRepu);
                cmd.Parameters.AddWithValue("@Parte", detalleOTE.Parte);
                cmd.Parameters.AddWithValue("@Pieza", detalleOTE.Pieza);
                cmd.Parameters.AddWithValue("@Cantidad", detalleOTE.Cantidad);

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


        public Boolean DeshabilitarOTE(EntOTE evaluacion)
        {
            SqlCommand cmd = null;
            Boolean deshabilita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitaOTE", cn); // Procedimiento almacenado
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros para deshabilitar la evaluación interna
                cmd.Parameters.AddWithValue("@CodigoTE", evaluacion.CodigoTE); // Codigo de la Evaluación Interna
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

        public List<string> ObtenerCodigosBus(string codigoCM)
        {
            List<string> buses = new List<string>();

            try
            {
                // Establece la conexión con la base de datos
                SqlConnection cn = Conexion.Instancia.Conectar();

                // Consulta SQL que busca buses asociados a un Código CM específico y cuyo estado sea 'Activo'
                string query = "SELECT DISTINCT BusCM " +
                               "FROM Contratomantenimiento " +
                               "WHERE CodigoCM = @CodigoCM AND Estado = 'Activo'"; // Filtramos por Código CM y Estado 'Activo'

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoCM", codigoCM); // Añadimos el parámetro de forma segura
                cn.Open();

                // Ejecutamos la consulta y leemos los resultados
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    buses.Add(reader["BusCM"].ToString()); // Añadimos cada bus encontrado a la lista
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lo lanzamos con un mensaje personalizado
                throw new Exception("Error al obtener los buses para el Código CM: " + ex.Message);
            }

            return buses;
        }

        // Método para obtener los códigos de los proveedores
        public List<string> ObtenerCodigosProveedor(string codigoCM)
        {
            List<string> proveedores = new List<string>();

            try
            {
                // Establece la conexión con la base de datos
                SqlConnection cn = Conexion.Instancia.Conectar();

                // Consulta SQL que busca proveedores asociados a un Código CM específico y cuyo estado sea 'Activo'
                string query = "SELECT DISTINCT ProveedorCM " +
                               "FROM Contratomantenimiento " +
                               "WHERE CodigoCM = @CodigoCM AND Estado = 'Activo'"; // Filtramos por Código CM y Estado 'Activo'

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@CodigoCM", codigoCM); // Añadimos el parámetro de forma segura
                cn.Open();

                // Ejecutamos la consulta y leemos los resultados
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    proveedores.Add(reader["ProveedorCM"].ToString()); // Añadimos cada proveedor encontrado a la lista
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lo lanzamos con un mensaje personalizado
                throw new Exception("Error al obtener los proveedores para el Código CM: " + ex.Message);
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

        public List<EntRepuesto> ListarRepuesto()
        {
            SqlCommand cmd = null;
            List<EntRepuesto> lista = new List<EntRepuesto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar(); //singleton
                cmd = new SqlCommand("spListaRepuesto", cn); // assuming stored procedure is named spListaRepuestos
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EntRepuesto repuesto = new EntRepuesto();
                    repuesto.Codigo = dr["CodigoR"].ToString();
                    repuesto.Categoria = dr["CategoriaR"].ToString();
                    repuesto.Marcarepuesto = dr["MarcarepuestoR"].ToString();
                    repuesto.Descripcion = dr["Descripcion"].ToString();
                    repuesto.Stock = Convert.ToInt32(dr["Stock"]);

                    lista.Add(repuesto);
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
    }
}
