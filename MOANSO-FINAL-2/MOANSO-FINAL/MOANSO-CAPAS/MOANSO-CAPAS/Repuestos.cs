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
    public partial class Repuestos : Form
    {
        private LogRepuesto _logRepuesto = new LogRepuesto();
        public Repuestos()
        {
            InitializeComponent();
            listarRepuestos();
            Deshabilitar();
            Contar();
            Mostrar();
            cmbCalidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMarca.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Mostrar()
        {
            try
            {
                // Obtener códigos de proveedores, marcas y categorías desde la Capa Lógica
                List<string> codigosProveedores = _logRepuesto.ObtenerCodigosProveedores();
                List<string> codigosMarcas = _logRepuesto.ObtenerCodigosMarcasRepuestos();
                List<string> codigosCategorias = _logRepuesto.ObtenerCodigosCategorias();

                // Cargar los ComboBox con los códigos obtenidos
                cmbProveedor.DataSource = codigosProveedores;
                cmbMarca.DataSource = codigosMarcas;
                cmbCategoria.DataSource = codigosCategorias;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en los ComboBox: " + ex.Message);
            }
        }

        public void Contar()
        {
            int totalRegistros = 0;
            EntRepuesto bus = new EntRepuesto();  // Si es necesario crear un objeto EntBus, aunque no lo usemos en la lógica
            LogRepuesto negocio = new LogRepuesto();  // Instanciamos la capa lógica

            // Llamamos al método de la capa lógica que llama a la capa de datos
            Boolean exito = negocio.ContarTotalRepuesto(bus, ref totalRegistros);

            if (exito)
            {
                // Si la operación fue exitosa, mostramos el total en un Label
                lbRepuestos.Text = $"{totalRegistros}";
            }
        }

        public void listarRepuestos()
        {
            dgvRepuestos.DataSource = LogRepuesto.Instancia.ListarRepuesto2();
        }

        private void Habilitar()
        {
            txtCodigo.Enabled = true;
            txtDescripcion.Enabled = true;
            cmbCalidad.Enabled = true;
            cmbCategoria.Enabled = true;
            cmbMarca.Enabled = true;
            cmbProveedor.Enabled = true;
            txtFechaAdquisicion.Enabled = true;
            txtFechaIngreso.Enabled = true;
            txtStock.Enabled = true;
            txtBuscar.Enabled = true;
            btnRegistrar.Enabled = true;
            btnModificar.Enabled = true;
            btnDeshabilitar.Enabled = true;
            btnOrdenar.Enabled = true;
        }

        private void Deshabilitar()
        {
            txtCodigo.Enabled = false;
            txtDescripcion.Enabled = false;
            cmbCalidad.Enabled = false;
            cmbCategoria.Enabled = false;
            cmbMarca.Enabled = false;
            cmbProveedor.Enabled = false;
            txtFechaAdquisicion.Enabled = false;
            txtFechaIngreso.Enabled = false;
            txtStock.Enabled = false;
            txtBuscar.Enabled = false;
            btnRegistrar.Enabled = false;
            btnModificar.Enabled = false;
            btnDeshabilitar.Enabled = false; ;
            btnOrdenar.Enabled = false;
        }

        private void LimpiarVariables()
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = " ";
            cmbCalidad.Text = "";
            cmbCategoria.Text = " ";
            cmbMarca.Text = "";
            cmbProveedor.Text = " ";
            txtFechaAdquisicion.Text = "";
            txtFechaIngreso.Text = " ";
            txtStock.Text = " ";
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            /*
            DateTime fechaSeleccionada = e.Start;

            // Formatear la fecha en el formato YYYY-MM-DD
            string fechaFormateada = fechaSeleccionada.ToString("yyyy-MM-dd");

            // Asignar la fecha formateada al TextBox
            txtFecha.Text = fechaFormateada;
            */
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntRepuesto c = new EntRepuesto();
                c.Codigo = txtCodigo.Text.Trim();
                c.Categoria = cmbCategoria.Text.Trim();
                c.Marcarepuesto = cmbMarca.Text.Trim();
                c.Descripcion = txtDescripcion.Text.Trim();
                c.Proveedor = cmbProveedor.Text.Trim();
                c.FechaAdquisicion = DateTime.Parse(txtFechaAdquisicion.Text.Trim());
                c.FechaIngreso = DateTime.Parse(txtFechaIngreso.Text.Trim());
                c.Stock = int.Parse(txtStock.Text.Trim());
                c.Estado = lbEstado.Text.Trim();
                LogRepuesto.Instancia.InsertaRepuesto(c);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            listarRepuestos();
            LimpiarVariables();
            Deshabilitar();
        }

        private void dgvRepuestos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow filaActual = dgvRepuestos.Rows[e.RowIndex];
            txtCodigo.Text = filaActual.Cells[0].Value.ToString();
            txtDescripcion.Text = filaActual.Cells[1].Value.ToString();
            cmbCategoria.Text = filaActual.Cells[2].Value.ToString();
            cmbMarca.Text = filaActual.Cells[3].Value.ToString();
            cmbProveedor.Text = filaActual.Cells[4].Value.ToString();
            txtFechaAdquisicion.Text = filaActual.Cells[5].Value.ToString();
            cmbCalidad.Text = filaActual.Cells[6].Value.ToString();
            txtFechaIngreso.Text = filaActual.Cells[7].Value.ToString();
            txtStock.Text = filaActual.Cells[8].Value.ToString();
            lbEstado.Text = filaActual.Cells[9].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                EntRepuesto c = new EntRepuesto();
                c.Codigo = txtCodigo.Text.Trim();
                c.Categoria = cmbCategoria.Text.Trim();
                c.Marcarepuesto = cmbMarca.Text.Trim();
                c.Descripcion = txtDescripcion.Text.Trim();
                c.Proveedor = cmbProveedor.Text.Trim();
                c.FechaAdquisicion = DateTime.Parse(txtFechaAdquisicion.Text.Trim());
                c.FechaIngreso = DateTime.Parse(txtFechaIngreso.Text.Trim());
                c.Stock = int.Parse(txtStock.Text.Trim());
                c.Estado = lbEstado.Text.Trim();
                LogRepuesto.Instancia.EditaRepuesto(c);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            listarRepuestos();
            LimpiarVariables();
            Deshabilitar();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código de repuesto a anular
                string codigo = Interaction.InputBox("Ingresa el CODIGO de Repuesto a Anular", "Deshabilitar Repuesto");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigo))
                {
                    // Instanciar objeto para manejar el repuesto
                    EntRepuesto c = new EntRepuesto();
                    c.Codigo = codigo.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Tomar el estado del repuesto desde el control cmbEstado

                    // Llamar al método para deshabilitar el repuesto
                    LogRepuesto.Instancia.DeshabilitarRepuesto(c);
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
                listarRepuestos();
                Deshabilitar();
            }


        }

        private BindingSource bindingSourceRepuesto = new BindingSource();

        public void FiltrarRepuesto(DataGridView dgvbus, string filtro)
        {
            try
            {
                DataTable dtbus = LogRepuesto.Instancia.ObtenerRepuestoFiltrados(filtro);  // Obtener los datos filtrados
                bindingSourceRepuesto.DataSource = dtbus;
                dgvbus.DataSource = bindingSourceRepuesto;
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
            FiltrarRepuesto(dgvRepuestos, filtro);
        }

        public void CargarRepuestoOrdenados(DataGridView dgvClientes)
        {
            try
            {
                DataTable dtBus = LogRepuesto.Instancia.ObtenerRepuestoOrdenado2();
                bindingSourceRepuesto.DataSource = dtBus;
                dgvClientes.DataSource = bindingSourceRepuesto;
                dgvClientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes ordenados: " + ex.Message);
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            CargarRepuestoOrdenados(dgvRepuestos);
        }
    }
}
