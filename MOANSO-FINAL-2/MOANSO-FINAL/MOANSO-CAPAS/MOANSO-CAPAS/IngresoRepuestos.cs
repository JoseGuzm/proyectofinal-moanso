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
    public partial class IngresoRepuestos : Form
    {
        private LogIngresoRepuesto _logNotaIngresoRepuestos = new LogIngresoRepuesto();
        public IngresoRepuestos()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbEstado.Visible = false;
            Codigo();
            cmbCodigoProve.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Codigo()
        {
            try
            {
                string codigoTE = lbCompra.Text;
                // Obtener los códigos de los proveedores desde la Capa Lógica
                List<string> codigosProveedores = _logNotaIngresoRepuestos.ObtenerCodigosProveedores(codigoTE);

                // Cargar el ComboBox con los códigos obtenidos
                cmbCodigoProve.DataSource = codigosProveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en el ComboBox: " + ex.Message);
            }
        }

        public void Numero2()
        {
            LogEvaluacionInterna ventaBLL = new LogEvaluacionInterna();
            string numBoleta = ventaBLL.GenerarNumeroBoleta2();

            lbCompra.Text = "OC 000" + numBoleta;
        }


        public void Numero()
        {
            LogEvaluacionInterna ventaBLL = new LogEvaluacionInterna(); 
            string numBoleta = ventaBLL.GenerarNumeroBoleta(); 

            lbIngreso.Text = "IR" + numBoleta;
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
            txtRecibidoR.Enabled = estado;
            txtAceptadoR.Enabled = estado;
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
            txtRecibidoR.Text = "";
            txtAceptadoR.Text = "";
        }

        private void LimpiarVariables2()
        {
            txtCodigoR.Text = "";
            txtNombreR.Text = " ";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = " ";
            txtRecibidoR.Text = "";
            txtAceptadoR.Text = "";
            txtPrecio.Text = "";
        }

        public void insertar()
        {
            try
            {
                string EICodigo = lbIngreso.Text;
                foreach (DataGridViewRow row in dgvRepuestos.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntIngresoRepuesto detalle = new EntIngresoRepuesto()
                    {
                        IRCodigo = EICodigo,  // Usamos el código obtenido del TextBox (ya convertido a int)
                        CodigoRepu = row.Cells["CodigoRepu"].Value.ToString(),
                        CantidadRecibida = Convert.ToInt32(row.Cells["CantidadRecibida"].Value),
                        CantidadAceptada = Convert.ToInt32(row.Cells["CantidadAceptada"].Value),
                        Precio = Convert.ToInt32(row.Cells["Precio"].Value),
                    };
                    LogIngresoRepuesto.Instancia.InsertaDetalleIngresoRepuesto(detalle);
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

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntIngresoRepuesto c = new EntIngresoRepuesto();
                c.CodigoIR = lbIngreso.Text.Trim();
                c.CodigoOC = lbCompra.Text.Trim();
                c.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                c.ProveedorIR = cmbCodigoProve.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogIngresoRepuesto.Instancia.InsertaIngresoRepuesto(c);
                insertar();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            LimpiarVariables();
            Cajas(false);
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código de ingreso de repuesto a anular
                string codigoIngresoRepuesto = Interaction.InputBox("Ingresa el CODIGO de Ingreso de Repuesto a Anular", "Deshabilitar Ingreso de Repuesto");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoIngresoRepuesto))
                {
                    // Instanciar objeto para manejar el ingreso de repuesto
                    EntIngresoRepuesto c = new EntIngresoRepuesto();
                    c.CodigoIR = codigoIngresoRepuesto.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado desde el control lbEstado

                    // Llamar al método para deshabilitar el ingreso de repuesto
                    LogIngresoRepuesto.Instancia.DeshabilitarIngresoRepuesto(c);
                }
                else
                {
                    MessageBox.Show("Operación cancelada o código no proporcionado.");
                }
            }
            catch (Exception ex)
            {
                // Mostrar el error con detalles adicionales
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
            string recibido = txtRecibidoR.Text;
            string aceptado = txtAceptadoR.Text;
            string precio = txtPrecio.Text;
            dgvRepuestos.Rows.Add(codigo, nombre, categoria, marca, recibido, aceptado, precio);
            LimpiarVariables2();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Cajas(true);
            Numero2();
            Numero();
            RepuestoOC();
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
                    EntProveedor bus = _logNotaIngresoRepuestos.ObtenerDatosProve(codigoBus);

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

        public void RepuestoOC()
        {
            try
            {
                string codigoOC = lbCompra.Text;  // Asumiendo que el Código OC está en un Label llamado lblCodigoOC

                // Crear una instancia de la Capa Lógica
                LogIngresoRepuesto logOrdenCompra = new LogIngresoRepuesto();

                // Obtener los repuestos para la Orden de Compra
                List<RepuestoOrdenCompra> repuestos = logOrdenCompra.Repuestos(codigoOC);

                if (repuestos != null && repuestos.Count > 0)
                {
                    // Limpiar el DataGridView antes de agregar los nuevos datos
                    dgvRepuestos.Rows.Clear();

                    // Recorrer la lista de repuestos y agregar una fila por cada repuesto
                    foreach (var repuesto in repuestos)
                    {
                        dgvRepuestos.Rows.Add(
                            repuesto.CodigoRep,
                            repuesto.Descripcion,
                            repuesto.CategoriaR,
                            repuesto.MarcarepuestoR,
                            repuesto.Cantidad);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron repuestos asociados a la Orden de Compra.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos de los repuestos: " + ex.Message);
            }

        }
    }
}
