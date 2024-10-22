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
    public partial class MarcaRepuesto : Form
    {
        private LogMarcaRepuesto _logMarcaRepuestos = new LogMarcaRepuesto();
        public MarcaRepuesto()
        {
            InitializeComponent();
            listarMarcaRepuesto();
            Deshabilitar();
            Contar();
            lbEstado.Visible = false;
            CodigoProve();
            cmbProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void CodigoProve()
        {
            try
            {
                List<string> codigosProveedores = _logMarcaRepuestos.ObtenerCodigosProveedores();
                cmbProveedor.DataSource = codigosProveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los proveedores: " + ex.Message);
            }
}

        public void Contar()
        {
            int totalRegistros = 0;
            EntMarcaRepuesto cat = new EntMarcaRepuesto();  // Si es necesario crear un objeto EntBus, aunque no lo usemos en la lógica
            LogMarcaRepuesto negocio = new LogMarcaRepuesto();  // Instanciamos la capa lógica

            // Llamamos al método de la capa lógica que llama a la capa de datos
            Boolean exito = negocio.ContarTotalMarcaRepuesto(cat, ref totalRegistros);

            if (exito)
            {
                // Si la operación fue exitosa, mostramos el total en un Label
                lbMarca.Text = $"{totalRegistros}";
            }
        }

        public void listarMarcaRepuesto()
        {
            dgvMarca.DataSource = LogMarcaRepuesto.Instancia.ListarMarcaRepuesto2();
        }

        private void Habilitar()
        {
            txtCodigo.Enabled = true;
            cmbProveedor.Enabled = true;
            txtDescripcion.Enabled = true;
            txtBuscar.Enabled = true;
            btnRegistrar.Enabled = true;
            btnModificar.Enabled = true;
            btnDeshabilitar.Enabled = true;
            btnOrdenar.Enabled = true;
        }

        private void Deshabilitar()
        {
            txtCodigo.Enabled = false;
            cmbProveedor.Enabled = false;
            txtDescripcion.Enabled = false;
            txtBuscar.Enabled = false;
            btnRegistrar.Enabled = false;
            btnModificar.Enabled = false;
            btnDeshabilitar.Enabled = false;
            btnOrdenar.Enabled = false;
        }

        private void LimpiarVariables()
        {
            txtCodigo.Text = "";
            cmbProveedor.Text = " ";
            txtDescripcion.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código de la marca de repuesto a anular
                string codigoMarcaRepuesto = Interaction.InputBox("Ingresa el CODIGO de Marca de Repuesto a Anular", "Deshabilitar Marca de Repuesto");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoMarcaRepuesto))
                {
                    // Instanciar objeto para manejar la marca de repuesto
                    EntMarcaRepuesto c = new EntMarcaRepuesto();
                    c.Codigo = codigoMarcaRepuesto.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado de la marca desde el control cmbEstado

                    // Llamar al método para deshabilitar la marca de repuesto
                    LogMarcaRepuesto.Instancia.DeshabilitarMarcaRepuesto(c);
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
                listarMarcaRepuesto();
                Deshabilitar();
            }

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntMarcaRepuesto c = new EntMarcaRepuesto();
                c.Codigo = txtCodigo.Text.Trim();
                c.Proveedor = cmbProveedor.Text.Trim();
                c.Descripcion = txtDescripcion.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogMarcaRepuesto.Instancia.InsertaMarcaRepuesto(c);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            listarMarcaRepuesto();
            LimpiarVariables();
            Deshabilitar();
        }

        private void dgvMarca_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow filaActual = dgvMarca.Rows[e.RowIndex];
            txtCodigo.Text = filaActual.Cells[0].Value.ToString();
            cmbProveedor.Text = filaActual.Cells[1].Value.ToString();
            txtDescripcion.Text = filaActual.Cells[2].Value.ToString();
            lbEstado.Text = filaActual.Cells[3].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                EntMarcaRepuesto c = new EntMarcaRepuesto();
                c.Codigo = txtCodigo.Text.Trim();
                c.Proveedor = cmbProveedor.Text.Trim();
                c.Descripcion = txtDescripcion.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogMarcaRepuesto.Instancia.EditaMarcaRepuesto(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            LimpiarVariables();
            listarMarcaRepuesto();
            Deshabilitar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar();
        }

        private BindingSource bindingSourceMarcaRepuesto = new BindingSource();

        public void FiltrarMarcaRepuesto(DataGridView dgvcat, string filtro)
        {
            try
            {
                DataTable dtcat = LogMarcaRepuesto.Instancia.ObtenerMarcaRepuestoFiltrados(filtro);
                bindingSourceMarcaRepuesto.DataSource = dtcat;
                dgvcat.DataSource = bindingSourceMarcaRepuesto;
                dgvcat.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar Bus: " + ex.Message);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();
            FiltrarMarcaRepuesto(dgvMarca, filtro);
        }

        public void CargarEspecialidadOrdenados(DataGridView dgvClientes)
        {
            try
            {
                DataTable dtBus = LogMarcaRepuesto.Instancia.ObtenerMarcaRepuestoOrdenado2();
                bindingSourceMarcaRepuesto.DataSource = dtBus;
                dgvClientes.DataSource = bindingSourceMarcaRepuesto;
                dgvClientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes ordenados: " + ex.Message);
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            CargarEspecialidadOrdenados(dgvMarca);
        }
    }
}
