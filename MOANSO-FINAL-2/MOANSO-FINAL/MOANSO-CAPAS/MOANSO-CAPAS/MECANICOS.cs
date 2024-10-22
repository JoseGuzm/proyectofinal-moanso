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
    public partial class MECANICOS : Form
    {
        private LogMecanico _logMecanico = new LogMecanico();
        public MECANICOS()
        {
            InitializeComponent();
            listarMecanico();
            Deshabilitar();
            Contar();
            codigo();
            cmbEspecialidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbExperiencia.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTurno.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void codigo()
        {
            try
            {
                // Obtener los códigos de especialidades desde la Capa Lógica
                List<string> codigosEspecialidades = _logMecanico.ObtenerCodigosEspecialidades();

                // Cargar los ComboBox con los códigos obtenidos
                cmbEspecialidad.DataSource = codigosEspecialidades;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en el ComboBox: " + ex.Message);
            }
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
                lbMecanico.Text = $"{totalRegistros}";
            }
        }

        public void listarMecanico()
        {
            dgvMecanicos.DataSource = LogBus.Instancia.ListarBus2();
        }

        private void Habilitar()
        {
            txtCodigo.Enabled = true;
            txtNombre.Enabled = true;
            txtDNI.Enabled = true;
            cmbEspecialidad.Enabled = true;
            txtDomicilio.Enabled = true;
            cmbExperiencia.Enabled = true;
            txtTelefono.Enabled = true;
            txtSueldo.Enabled = true;
            cmbTurno.Enabled = true;
            txtFecha.Enabled = true;
            txtBuscar.Enabled = true;
            btnRegistrar.Enabled = true;
            btnModificar.Enabled = true;
            btnDeshabilitar.Enabled = true;
            btnOrdenar.Enabled = true;
        }

        private void Deshabilitar()
        {
            txtCodigo.Enabled = false;
            txtNombre.Enabled = false;
            txtDNI.Enabled = false;
            cmbEspecialidad.Enabled = false;
            txtDomicilio.Enabled = false;
            cmbExperiencia.Enabled = false;
            txtTelefono.Enabled = false;
            txtSueldo.Enabled = false;
            cmbTurno.Enabled = false;
            txtFecha.Enabled = false;
            txtBuscar.Enabled = false;
            btnRegistrar.Enabled = false;
            btnModificar.Enabled = false;
            btnDeshabilitar.Enabled = false;
            btnOrdenar.Enabled = false;
        }

        private void LimpiarVariables()
        {
            txtCodigo.Text = "";
            txtNombre.Text = " ";
            txtDNI.Text = "";
            cmbEspecialidad.Text = " ";
            txtDomicilio.Text = "";
            cmbExperiencia.Text = " ";
            txtTelefono.Text = "";
            txtSueldo.Text = " ";
            cmbTurno.Text = "";
            txtFecha.Text = " ";
            txtBuscar.Text = " ";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntMecanico c = new EntMecanico();
                c.Codigo = txtCodigo.Text.Trim();
                c.Especialidad = cmbEspecialidad.Text.Trim();
                c.Nombre = txtNombre.Text.Trim();
                c.DNI = txtDNI.Text.Trim();
                c.Domicilio = txtDomicilio.Text.Trim();
                c.Experiencia = cmbExperiencia.Text.Trim();
                c.Telefono = txtTelefono.Text.Trim();
                c.Sueldo = double.Parse(txtSueldo.Text.Trim());
                c.Turno = cmbTurno.Text.Trim();
                c.FechaContrato = DateTime.Parse(txtFecha.Text.Trim());
                c.Estado = lbEstado.Text.Trim();
                LogMecanico.Instancia.InsertaMecanico(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            listarMecanico();
            LimpiarVariables();
            Deshabilitar();
        }

        private void dgvMecanicos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow filaActual = dgvMecanicos.Rows[e.RowIndex];
            txtCodigo.Text = filaActual.Cells[0].Value.ToString();
            cmbEspecialidad.Text = filaActual.Cells[1].Value.ToString();
            txtNombre.Text = filaActual.Cells[2].Value.ToString();
            txtDNI.Text = filaActual.Cells[3].Value.ToString();
            txtDomicilio.Text = filaActual.Cells[4].Value.ToString();
            cmbExperiencia.Text = filaActual.Cells[5].Value.ToString();
            txtTelefono.Text = filaActual.Cells[6].Value.ToString();
            txtSueldo.Text = filaActual.Cells[7].Value.ToString();
            cmbTurno.Text = filaActual.Cells[8].Value.ToString();
            txtFecha.Text = filaActual.Cells[9].Value.ToString();
            lbEstado.Text = filaActual.Cells[10].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                EntMecanico c = new EntMecanico();
                c.Codigo = txtCodigo.Text.Trim();
                c.Especialidad = cmbEspecialidad.Text.Trim();
                c.Nombre = txtNombre.Text.Trim();
                c.DNI = txtDNI.Text.Trim();
                c.Domicilio = txtDomicilio.Text.Trim();
                c.Experiencia = cmbExperiencia.Text.Trim();
                c.Telefono = txtTelefono.Text.Trim();
                c.Sueldo = double.Parse(txtSueldo.Text.Trim());
                c.Turno = cmbTurno.Text.Trim();
                c.FechaContrato = DateTime.Parse(txtFecha.Text.Trim());
                c.Estado = lbEstado.Text.Trim();
                LogMecanico.Instancia.EditaMecanico(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            LimpiarVariables();
            listarMecanico();
            Deshabilitar();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código del mecánico a anular
                string codigoMecanico = Interaction.InputBox("Ingresa el CODIGO del Mecánico a Anular", "Deshabilitar Mecánico");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoMecanico))
                {
                    // Instanciar objeto para manejar el mecánico
                    EntMecanico c = new EntMecanico();
                    c.Codigo = codigoMecanico.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado del mecánico desde el control lbEstado

                    // Llamar al método para deshabilitar el mecánico
                    LogMecanico.Instancia.DeshabilitarMecanico(c);
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
                listarMecanico();
                Deshabilitar();
            }

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar();
        }

        private BindingSource bindingSourceMecanico = new BindingSource();

        public void FiltrarMecanico(DataGridView dgvbus, string filtro)
        {
            try
            {
                DataTable dtbus = LogMecanico.Instancia.ObtenerMecanicoFiltrados(filtro);  // Obtener los datos filtrados
                bindingSourceMecanico.DataSource = dtbus;
                dgvbus.DataSource = bindingSourceMecanico;
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
            FiltrarMecanico(dgvMecanicos, filtro);
        }

        public void CargarMecanicoOrdenados(DataGridView dgvClientes)
        {
            try
            {
                DataTable dtBus = LogMecanico.Instancia.ObtenerMecanicoOrdenado2();
                bindingSourceMecanico.DataSource = dtBus;
                dgvClientes.DataSource = bindingSourceMecanico;
                dgvClientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes ordenados: " + ex.Message);
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            CargarMecanicoOrdenados(dgvMecanicos);
        }
    }
}
