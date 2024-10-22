using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaLogica;
using Microsoft.VisualBasic;

namespace MOANSO_CAPAS
{
    public partial class Bus : Form
    {
        public Bus()
        {
            InitializeComponent();
            listarBus();
            Deshabilitar();
            Contar();
            lbEstado.Visible = false;
            cmbPiso.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Contar()
        {
            int totalRegistros = 0;
            EntBus bus = new EntBus();  // Si es necesario crear un objeto EntBus, aunque no lo usemos en la lógica
            LogBus negocio = new LogBus();  // Instanciamos la capa lógica

            // Llamamos al método de la capa lógica que llama a la capa de datos
            Boolean exito = negocio.ContarTotalBuses(bus, ref totalRegistros);

            if (exito)
            {
                // Si la operación fue exitosa, mostramos el total en un Label
                lbBus.Text = $"{totalRegistros}";
            }
        }

        public void listarBus()
        {
            dgvBus.DataSource = LogBus.Instancia.ListarBus2();
        }

        private void Habilitar()
        {
            txtCodigo.Enabled = true;
            txtMarca.Enabled = true;
            txtModelo.Enabled = true;
            cmbPiso.Enabled = true;
            txtPlaca.Enabled = true;
            txtChasis.Enabled = true;
            txtMotor.Enabled = true;
            txtCapacidad.Enabled = true;
            txtTipoMotor.Enabled = true;
            txtCombustible.Enabled = true;
            txtFechaAquisicion.Enabled = true;
            txtKilometraje.Enabled = true;
            txtBuscar.Enabled = true;
            btnRegistrar.Enabled = true;
            btnModificar.Enabled = true;
            btnDeshabilitar.Enabled = true;
            btnOrdenar.Enabled = true;
        }

        private void Deshabilitar()
        {
            txtCodigo.Enabled = false;
            txtMarca.Enabled = false;
            txtModelo.Enabled = false;
            cmbPiso.Enabled = false;
            txtPlaca.Enabled = false;
            txtChasis.Enabled = false; ;
            txtMotor.Enabled = false;
            txtCapacidad.Enabled = false;
            txtTipoMotor.Enabled = false;
            txtCombustible.Enabled = false; ;
            txtFechaAquisicion.Enabled = false;
            txtKilometraje.Enabled = false;
            txtBuscar.Enabled = false;
            btnRegistrar.Enabled = false;
            btnModificar.Enabled = false;
            btnDeshabilitar.Enabled = false;
            btnOrdenar.Enabled = false;
        }

        private void LimpiarVariables()
        {
            txtCodigo.Text = "";
            txtMarca.Text = " ";
            txtModelo.Text = "";
            cmbPiso.Text = " ";
            txtPlaca.Text = "";
            txtChasis.Text = " ";
            txtMotor.Text = "";
            txtCapacidad.Text = " ";
            txtTipoMotor.Text = "";
            txtCombustible.Text = " ";
            txtFechaAquisicion.Text = "";
            txtKilometraje.Text = " ";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntBus c = new EntBus();
                c.BusB = txtCodigo.Text.Trim();
                c.Marca = txtMarca.Text.Trim();
                c.Modelo = txtModelo.Text.Trim();
                c.PisoBus = cmbPiso.Text.Trim(); 
                c.NPlaca = txtPlaca.Text.Trim();
                c.NChasis = txtChasis.Text.Trim();
                c.NMotor = txtMotor.Text.Trim();
                c.Capacidad = int.Parse(txtCapacidad.Text.Trim());
                c.TipoMotor = txtTipoMotor.Text.Trim();
                c.Combustible = txtCombustible.Text.Trim();
                c.FechaAdquisicion = DateTime.Parse(txtFechaAquisicion.Text.Trim());
                c.Kilometraje = int.Parse(txtKilometraje.Text.Trim());
                c.Estado = lbEstado.Text.Trim();
                LogBus.Instancia.InsertaCliente(c);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            listarBus();
            LimpiarVariables();
            Deshabilitar();
        }

        private void dgvBus_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow filaActual = dgvBus.Rows[e.RowIndex];
            txtCodigo.Text = filaActual.Cells[0].Value.ToString();
            txtMarca.Text = filaActual.Cells[1].Value.ToString();
            txtModelo.Text = filaActual.Cells[2].Value.ToString();
            cmbPiso.Text = filaActual.Cells[3].Value.ToString();
            txtPlaca.Text = filaActual.Cells[4].Value.ToString();
            txtChasis.Text = filaActual.Cells[5].Value.ToString();
            txtMotor.Text = filaActual.Cells[6].Value.ToString();
            txtCapacidad.Text = filaActual.Cells[7].Value.ToString(); 
            txtTipoMotor.Text = filaActual.Cells[8].Value.ToString();
            txtCombustible.Text = filaActual.Cells[9].Value.ToString();
            txtFechaAquisicion.Text = filaActual.Cells[10].Value.ToString();
            txtKilometraje.Text = filaActual.Cells[11].Value.ToString();
            lbEstado.Text = filaActual.Cells[12].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                EntBus c = new EntBus();
                c.BusB = txtCodigo.Text.Trim();
                c.Marca = txtMarca.Text.Trim();
                c.Modelo = txtModelo.Text.Trim();
                c.PisoBus = cmbPiso.Text.Trim(); ;
                c.NPlaca = txtPlaca.Text.Trim();
                c.NChasis = txtChasis.Text.Trim();
                c.NMotor = txtMotor.Text.Trim();
                c.Capacidad = int.Parse(txtCapacidad.Text.Trim());
                c.TipoMotor = txtTipoMotor.Text.Trim();
                c.Combustible = txtCombustible.Text.Trim();
                c.FechaAdquisicion = DateTime.Parse(txtFechaAquisicion.Text.Trim());
                c.Kilometraje = int.Parse(txtKilometraje.Text.Trim());
                c.Estado = lbEstado.Text.Trim();
                LogBus.Instancia.EditaBus(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            LimpiarVariables();
            listarBus();
            Deshabilitar();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código de bus a anular
                string codigoBus = Interaction.InputBox("Ingresa el CODIGO de Bus a Anular", "Deshabilitar Bus");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoBus))
                {
                    // Instanciar objeto para manejar el bus
                    EntBus c = new EntBus();
                    c.BusB = codigoBus.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado desde el control lbEstado

                    // Llamar al método para deshabilitar el bus
                    LogBus.Instancia.DeshabilitarBus(c);
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
                listarBus();
                Deshabilitar();
            }

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar();
        }

        private BindingSource bindingSourceBus = new BindingSource();

        public void FiltrarBus(DataGridView dgvbus, string filtro)
        {
            try
            {
                DataTable dtbus = LogBus.Instancia.ObtenerClientesFiltrados(filtro);  // Obtener los datos filtrados
                bindingSourceBus.DataSource = dtbus;
                dgvbus.DataSource = bindingSourceBus;
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
            FiltrarBus(dgvBus, filtro);
        }

        private BindingSource bindingSourceClientes = new BindingSource();

        public void CargarBusOrdenados(DataGridView dgvClientes)
        {
            try
            {
                DataTable dtBus = LogBus.Instancia.ObtenerBusOrdenado2();
                bindingSourceClientes.DataSource = dtBus;
                dgvClientes.DataSource = bindingSourceClientes;
                dgvClientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes ordenados: " + ex.Message);
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            CargarBusOrdenados(dgvBus);
        }
    }  
}