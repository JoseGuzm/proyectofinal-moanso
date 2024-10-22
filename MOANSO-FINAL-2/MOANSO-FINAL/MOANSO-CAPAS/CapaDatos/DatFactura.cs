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
    public class DatFactura
    {
        #region sigleton
        //Patron Singleton
        // Variable estática para la instancia
        private static readonly DatFactura _instancia = new DatFactura();
        //privado para evitar la instanciación directa
        public static DatFactura Instancia
        {
            get
            {
                return DatFactura._instancia;
            }
        }
        #endregion singleton


        #region metodos
        public List<EntFactura> ListarFactura()
        {
            SqlCommand cmd = null;
            List<EntFactura> lista = new List<EntFactura>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar(); //singleton
                cmd = new SqlCommand("spListaFactura", cn); 
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EntFactura factura = new EntFactura();

                    factura.CodigoFactura = Convert.ToInt32(dr["CodigoFactura"]);
                    factura.CodigoOC = dr["CodigoOC"].ToString();
                    factura.Fecha = Convert.ToDateTime(dr["Fecha"]);
                    factura.TOTAL = Convert.ToDouble(dr["TOTAL"]);

                    lista.Add(factura);
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

        public Boolean ContarRegistro(ref int totalRegistros)
        {
            SqlCommand cmd = null;
            Boolean exito = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                string consulta = "SELECT COUNT(*) FROM Factura";  // Consulta para contar los registros

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
        private SqlConnection _conexion = Conexion.Instancia.Conectar();

        public EntFactura ObtenerDetallesFactura(int codigoFactura)
        {
            EntFactura factura = null;

            try
            {
                // Abrir la conexión
                _conexion.Open();

                // Consulta SQL para obtener los datos de la factura y sus tablas relacionadas
                string query = @"
            SELECT 
                f.CodigoFactura, f.Fecha AS FechaFactura,
                o.CodigoPro, o.FormaPago,
                d.CodigoRep, d.Cantidad, d.Precio,
                p.NombreEmpresa, p.RazonSocial, p.RUC, p.Direccion, p.Telefono,
                r.Descripcion AS DescripcionRepuesto, r.CategoriaR, r.MarcarepuestoR
            FROM 
                Factura f
            INNER JOIN 
                OrdenCompra o ON f.CodigoFactura = o.OPCodigo
            INNER JOIN 
                DetalleOrdenCompra d ON o.CodigoOC = d.OCCompra
            INNER JOIN 
                Proveedor p ON o.CodigoPro = p.CodigoP
            INNER JOIN 
                Repuesto r ON d.CodigoRep = r.CodigoR
            WHERE 
                f.CodigoFactura = @CodigoFactura";

                // Crear el comando SQL
                SqlCommand cmd = new SqlCommand(query, _conexion);
                cmd.Parameters.AddWithValue("@CodigoFactura", codigoFactura);

                // Ejecutar la consulta y obtener los resultados
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Crear una instancia de la factura con los datos obtenidos
                    factura = new EntFactura
                    {
                        CodigoFactura = Convert.ToInt32(reader["CodigoFactura"]), // Convertir a int
                        Fecha = Convert.ToDateTime(reader["FechaFactura"]),
                        // Obtener los datos de OrdenCompra
                        CodigoPro = reader["CodigoPro"].ToString(),
                        FormaPago = reader["FormaPago"].ToString(),
                        // Obtener los datos de DetalleOrdenCompra
                        CodigoRep = reader["CodigoRep"].ToString(),
                        Cantidad = Convert.ToInt32(reader["Cantidad"]),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        // Obtener los datos de Proveedor
                        NombreEmpresa = reader["NombreEmpresa"].ToString(),
                        RazonSocial = reader["RazonSocial"].ToString(),
                        RUC = reader["RUC"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        // Obtener los datos de Repuesto
                        DescripcionRepuesto = reader["DescripcionRepuesto"].ToString(),
                        CategoriaR = reader["CategoriaR"].ToString(),
                        MarcarepuestoR = reader["MarcarepuestoR"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al obtener los detalles de la factura: " + ex.Message);
            }
            finally
            {
                // Cerrar la conexión siempre, incluso si ocurre un error
                //Conexion.Instancia.Cerrar(_conexion);
            }

            return factura;
        }



        public List<EntFactura> ObtenerDetallesRepuesto(int codigoFactura)
        {
            List<EntFactura> listaDetalles = new List<EntFactura>();
            SqlConnection conexion = Conexion.Instancia.Conectar();

            try
            {
                // Consulta para obtener los detalles de Repuesto y DetalleOrdenCompra
                string query = @"
            SELECT 
                r.Descripcion AS DescripcionRepuesto, 
                r.CategoriaR, 
                r.MarcarepuestoR,
                d.Cantidad,
                d.Precio
            FROM 
                DetalleOrdenCompra d
            INNER JOIN 
                Repuesto r ON d.CodigoRep = r.CodigoR
            WHERE 
                d.OCCompra = (SELECT OPCodigo FROM Factura WHERE CodigoFactura = @CodigoFactura)";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@CodigoFactura", codigoFactura);

                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Crear un objeto DetalleRepuesto y agregarlo a la lista
                    listaDetalles.Add(new EntFactura
                    {
                        DescripcionRepuesto = reader["DescripcionRepuesto"].ToString(),
                        CategoriaR = reader["CategoriaR"].ToString(),
                        MarcarepuestoR = reader["MarcarepuestoR"].ToString(),
                        Cantidad = Convert.ToInt32(reader["Cantidad"]),
                        Precio = Convert.ToDecimal(reader["Precio"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de repuestos: " + ex.Message);
            }
            return listaDetalles;
        }



        #endregion metodos
    }
}