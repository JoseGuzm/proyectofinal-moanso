using CapaEntidad;
using CapaLogica;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MOANSO_CAPAS
{
    public partial class OrdenCompra : Form
    {
        private LogOrdenCompra _logOrdenCompra = new LogOrdenCompra();
        public OrdenCompra()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbEstado.Visible = false;
            lbFactura.Visible = false;
            Codigo();
            cmbCodigoProve.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPago.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Codigo()
        {
            try
            {
                
                // Obtener los proveedores desde la Capa Lógica
                List<string> proveedores = _logOrdenCompra.ObtenerProveedores();

                // Cargar el ComboBox con los proveedores obtenidos
                cmbCodigoProve.DataSource = proveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en el ComboBox: " + ex.Message);
            }
        }

        public double SumaTotal()
        {
            // Obtener el número de filas de la tabla
            int contar = dgvRepuestos.RowCount;
            double suma = 0;

            // Recorrer las filas de la tabla y sumar los valores de la columna 3
            for (int i = 0; i < contar; i++)
            {
                // Obtener el valor de la celda y convertirlo a double
                suma += Convert.ToDouble(dgvRepuestos.Rows[i].Cells[6].Value);
            }

            return suma;
        }

        public double RestaTotal()
        {
            // Obtener el número de filas de la tabla
            int contar = dgvRepuestos.RowCount;
            double suma = 0;

            // Recorrer las filas de la tabla y sumar los valores de la columna 3
            for (int i = 0; i < contar; i++)
            {
                // Obtener el valor de la celda y convertirlo a double
                suma -= Convert.ToDouble(dgvRepuestos.Rows[i].Cells[6].Value);
            }

            return suma;
        }

        public void Numero2()
        {
            LogOrdenCompra ventaBLL = new LogOrdenCompra();
            string numBoleta = ventaBLL.GenerarNumeroBoleta2();

            lbPedido.Text = "OP 000" + numBoleta;
        }

        public void Numero()
        {
            LogOrdenCompra ventaBLL = new LogOrdenCompra();
            string numBoleta = ventaBLL.GenerarNumeroBoleta();

            lbCompra.Text = "OC" + numBoleta;
        }


        public void Cajas(Boolean estado)
        {
            cmbCodigoProve.Enabled = estado;
            txtNombre.Enabled = estado;
            txtRUC.Enabled = estado;
            txtRazon.Enabled = estado;
            txtTipo.Enabled = estado;
            txtTelefono.Enabled = estado;
            txtCodigoR.Enabled = estado;
            txtNombreR.Enabled = estado;
            txtCategoriaR.Enabled = estado;
            txtMarcaR.Enabled = estado;
            txtPrecioR.Enabled = estado;
            txtCantidadR.Enabled = estado;
            cmbPago.Enabled = estado;
            btnRegistrar.Enabled = estado;
            btnAnular.Enabled = estado;
        }

        private void LimpiarVariables()
        {
            cmbCodigoProve.Text = "";
            txtNombre.Text = " ";
            txtRUC.Text = "";
            txtRazon.Text = " ";
            txtTipo.Text = "";
            txtTelefono.Text = " ";
            txtCodigoR.Text = "";
            txtNombreR.Text = " ";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = " ";
            txtPrecioR.Text = "";
            txtCantidadR.Text = "";
            cmbPago.Text = "";
        }

        private void LimpiarVariables2()
        { 
            txtCodigoR.Text = "";
            txtNombreR.Text = " ";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = " ";
            txtPrecioR.Text = "";
            txtCantidadR.Text = "";
        }

        public void insertar()
        {
            try
            {
                string EICodigo = lbCompra.Text;
                foreach (DataGridViewRow row in dgvRepuestos.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntOrdenCompra detalle = new EntOrdenCompra()
                    {
                        OCCompra = EICodigo,  // Usamos el código obtenido del TextBox (ya convertido a int)
                        CodigoRep = row.Cells["CodigoRep"].Value.ToString(),
                        Cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value),
                        Precio = Convert.ToInt32(row.Cells["Precio"].Value),
                    };
                    LogOrdenCompra.Instancia.InsertaDetalleOrdenCompra(detalle);
                }
                MessageBox.Show("Datos insertados correctamente.");
            }
            catch (FormatException)
            {
                MessageBox.Show("El código de evaluación interna debe ser un número válido.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar los datos: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntOrdenCompra c = new EntOrdenCompra();
                c.CodigoOC = lbCompra.Text.Trim();
                c.OPCodigo = lbPedido.Text.Trim();
                c.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                c.CodigoPro = cmbCodigoProve.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                c.FormaPago = cmbPago.Text.Trim();
                c.Total = int.Parse(lbTOTAL.Text.Trim());
                LogOrdenCompra.Instancia.InsertaOrdenCompra(c);
                insertar();
                EntFactura f = new EntFactura();
                f.CodigoFactura = int.Parse(lbFactura.Text.Trim());
                f.CodigoOC = lbCompra.Text.Trim();
                f.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                f.TOTAL = int.Parse(lbTOTAL.Text.Trim());
                LogOrdenCompra.Instancia.InsertaFactura(f);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            LimpiarVariables();
            Cajas(false);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Cajas(true);
            Numero();
            Numero2();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código de la orden de compra a anular
                string codigoOC = Interaction.InputBox("Ingresa el CODIGO de Orden de Compra a Anular", "Deshabilitar Orden de Compra");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoOC))
                {
                    // Instanciar objeto para manejar la orden de compra
                    EntOrdenCompra c = new EntOrdenCompra();
                    c.CodigoOC = codigoOC.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado de la orden desde el control lbEstado

                    // Llamar al método para deshabilitar la orden de compra
                    LogOrdenCompra.Instancia.DeshabilitarOrdenCompra(c);
                }
                else
                {
                    MessageBox.Show("Operación cancelada o código no proporcionado.");
                }
            }
            catch (Exception ex)
            {
                // Mostrar el error con detalles adicionales (opcionalmente)
                MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                // Limpiar variables y controles de la interfaz
                LimpiarVariables();
                Cajas(false);
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigoR.Text;
            string nombre = txtNombreR.Text;
            string categoria = txtCategoriaR.Text;
            string marca = txtMarcaR.Text;
            string precio = txtPrecioR.Text;
            string cantidad = txtCantidadR.Text;
            dgvRepuestos.Rows.Add(codigo, nombre, categoria, marca, precio, cantidad);

            foreach (DataGridViewRow row in dgvRepuestos.Rows)
            {
                // Asegurarse de que la fila no sea una fila nueva o vacía
                if (!row.IsNewRow)
                {
                    // Obtener los valores de las columnas que deseas multiplicar (asegúrate de que sean numéricos)
                    double num1 = Convert.ToDouble(row.Cells[4].Value);  // Suponiendo que la primera columna es la 1
                    double num2 = Convert.ToDouble(row.Cells[5].Value);  // Suponiendo que la segunda columna es la 2

                    // Realizar la multiplicación
                    double resultado = num1 * num2;

                    // Colocar el resultado en la tercera columna (cambia el índice según sea necesario)
                    row.Cells[6].Value = resultado;  // Suponiendo que la tercera columna es la 3
                }
            }
            lbTOTAL.Text = SumaTotal().ToString();
            LimpiarVariables2();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (dgvRepuestos.SelectedRows.Count > 0)
            {
                // Eliminar la fila seleccionada (sólo filas completas, no la fila nueva)
                foreach (DataGridViewRow row in dgvRepuestos.SelectedRows)
                {
                    // Asegúrate de que no sea la fila nueva que aparece al final
                    if (!row.IsNewRow)
                    {
                        dgvRepuestos.Rows.Remove(row);
                    }
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna fila para eliminar.");
            }
            lbTOTAL.Text = RestaTotal().ToString();
            LimpiarVariables2();
        }

        private void cmbCodigoProve_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Obtener el código del bus seleccionado
                    string codigoBus = cmbCodigoProve.SelectedItem.ToString();

                    // Obtener los datos del bus desde la capa lógica
                    EntProveedor bus = _logOrdenCompra.ObtenerDatosProve(codigoBus);

                    if (bus != null)
                    {
                        // Llenar los TextBox con los datos del bus
                        txtNombre.Text = bus.Nombre;
                        txtRUC.Text = bus.RUC;
                        txtRazon.Text = bus.Razon;
                        txtTipo.Text = bus.Tipo;
                        txtTelefono.Text = bus.Telefono;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron detalles para el bus seleccionado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos del bus: " + ex.Message);
            }
        }
    }
}
