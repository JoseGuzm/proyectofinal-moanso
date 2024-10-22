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
    public partial class Proveedor : Form
    {
        public Proveedor()
        {
            InitializeComponent();
            listarProveedor();
            Deshabilitar();
            Contar();
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            lbEstado.Visible = false;
        }

        public void Contar()
        {
            int totalRegistros = 0;
            EntProveedor bus = new EntProveedor();  // Si es necesario crear un objeto EntBus, aunque no lo usemos en la lógica
            LogProveedor negocio = new LogProveedor();  // Instanciamos la capa lógica

            // Llamamos al método de la capa lógica que llama a la capa de datos
            Boolean exito = negocio.ContarTotalProveedor(bus, ref totalRegistros);

            if (exito)
            {
                // Si la operación fue exitosa, mostramos el total en un Label
                lbProveedor.Text = $"{totalRegistros}";
            }
        }

        public void listarProveedor()
        {
            dgvProveedor.DataSource = LogProveedor.Instancia.ListarProveedor2();
        }

        private void Habilitar()
        {
            txtCodigo.Enabled = true;
            txtRazon.Enabled = true;
            txtNombre.Enabled = true;
            cmbTipo.Enabled = true;
            txtRUC.Enabled = true;
            txtDireccion.Enabled = true;
            txtTelefono.Enabled = true;
            txtBuscar.Enabled = true;
            btnRegistrar.Enabled = true;
            btnModificar.Enabled = true;
            btnDeshabilitar.Enabled = true;
            btnOrdenar.Enabled = true;
        }

        private void Deshabilitar()
        {
            txtCodigo.Enabled = false;
            txtRazon.Enabled = false;
            txtNombre.Enabled = false;
            cmbTipo.Enabled = false;
            txtRUC.Enabled = false;
            txtDireccion.Enabled = false; ;
            txtTelefono.Enabled = false;
            txtBuscar.Enabled = false;
            btnRegistrar.Enabled = false;
            btnModificar.Enabled = false;
            btnDeshabilitar.Enabled = false;
            btnOrdenar.Enabled = false;
        }

        private void LimpiarVariables()
        {
            txtCodigo.Text = "";
            txtRazon.Text = " ";
            txtNombre.Text = "";
            cmbTipo.Text = " ";
            txtRUC.Text = "";
            txtDireccion.Text = " ";
            txtTelefono.Text = "";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntProveedor c = new EntProveedor();
                c.Codigo = txtCodigo.Text.Trim();
                c.Nombre = txtNombre.Text.Trim();
                c.Razon = txtRazon.Text.Trim();
                c.Tipo = cmbTipo.Text.Trim();
                c.RUC = txtRUC.Text.Trim();
                c.Direccion = txtDireccion.Text.Trim();
                c.Telefono = txtTelefono.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogProveedor.Instancia.InsertaProveedor(c);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            listarProveedor();
            LimpiarVariables();
            Deshabilitar();
        }

        private void dgvProveedor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow filaActual = dgvProveedor.Rows[e.RowIndex];
            txtCodigo.Text = filaActual.Cells[0].Value.ToString();
            txtNombre.Text = filaActual.Cells[1].Value.ToString();
            txtRazon.Text = filaActual.Cells[2].Value.ToString();
            cmbTipo.Text = filaActual.Cells[3].Value.ToString();
            txtRUC.Text = filaActual.Cells[4].Value.ToString();
            txtDireccion.Text = filaActual.Cells[5].Value.ToString();
            txtTelefono.Text = filaActual.Cells[6].Value.ToString();
            lbEstado.Text = filaActual.Cells[7].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                EntProveedor c = new EntProveedor();
                c.Codigo = txtCodigo.Text.Trim();
                c.Nombre = txtNombre.Text.Trim();
                c.Razon = txtRazon.Text.Trim();
                c.Tipo = cmbTipo.Text.Trim();
                c.RUC = txtRUC.Text.Trim();
                c.Direccion = txtDireccion.Text.Trim();
                c.Telefono = txtTelefono.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogProveedor.Instancia.EditaProveedor(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            LimpiarVariables();
            listarProveedor();
            Deshabilitar();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código del proveedor a anular
                string codigo = Interaction.InputBox("Ingresa el CODIGO del Proveedor a Anular", "Deshabilitar Proveedor");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigo))
                {
                    // Instanciar objeto para manejar el proveedor
                    EntProveedor c = new EntProveedor();
                    c.Codigo = codigo.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado del proveedor desde el control lbEstado

                    // Llamar al método para deshabilitar el proveedor
                    LogProveedor.Instancia.DeshabilitarProveedor(c);
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
                listarProveedor();
                Deshabilitar();
            }

        }

        private BindingSource bindingSourceProveedor = new BindingSource();

        public void FiltrarProveedor(DataGridView dgvbus, string filtro)
        {
            try
            {
                DataTable dtbus = LogProveedor.Instancia.ObtenerProveedorFiltrados(filtro);  // Obtener los datos filtrados
                bindingSourceProveedor.DataSource = dtbus;
                dgvbus.DataSource = bindingSourceProveedor;
                dgvbus.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar Bus: " + ex.Message);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();
            FiltrarProveedor(dgvProveedor, filtro);
        }

        public void CargarBusOrdenados(DataGridView dgvClientes)
        {
            try
            {
                DataTable dtBus = LogProveedor.Instancia.ObtenerProveedorOrdenado2();
                bindingSourceProveedor.DataSource = dtBus;
                dgvClientes.DataSource = bindingSourceProveedor;
                dgvClientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes ordenados: " + ex.Message);
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            CargarBusOrdenados(dgvProveedor);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar();
        }
    }
}
