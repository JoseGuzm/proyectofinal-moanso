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
    public partial class Especialidad : Form
    {
        public Especialidad()
        {
            InitializeComponent();
            listarEspecialidad();
            Deshabilitar();
            Contar();
            lbEstado.Visible = false;
        }

        public void Contar()
        {
            int totalRegistros = 0;
            EntEspecialidad cat = new EntEspecialidad();  // Si es necesario crear un objeto EntBus, aunque no lo usemos en la lógica
            LogEspecialidad negocio = new LogEspecialidad();  // Instanciamos la capa lógica

            // Llamamos al método de la capa lógica que llama a la capa de datos
            Boolean exito = negocio.ContarTotalEspecialidad(cat, ref totalRegistros);

            if (exito)
            {
                // Si la operación fue exitosa, mostramos el total en un Label
                lbEspecialidad.Text = $"{totalRegistros}";
            }
        }

        public void listarEspecialidad()
        {
            dgvEspecialidad.DataSource = LogCategoria.Instancia.ListarCategoria2();
        }

        private void Habilitar()
        {
            txtCodigo.Enabled = true;
            txtNombre.Enabled = true;
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
            txtNombre.Enabled = false;
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
            txtNombre.Text = " ";
            txtDescripcion.Text = "";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntEspecialidad c = new EntEspecialidad();
                c.Codigo = txtCodigo.Text.Trim();
                c.Nombre = txtNombre.Text.Trim();
                c.Descripcion = txtDescripcion.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogEspecialidad.Instancia.InsertaEspecialidad(c);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            listarEspecialidad();
            LimpiarVariables();
            Deshabilitar();
        }

        private void dgvEspecialidad_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow filaActual = dgvEspecialidad.Rows[e.RowIndex];
            txtCodigo.Text = filaActual.Cells[0].Value.ToString();
            txtNombre.Text = filaActual.Cells[1].Value.ToString();
            txtDescripcion.Text = filaActual.Cells[2].Value.ToString();
            lbEstado.Text = filaActual.Cells[3].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                EntEspecialidad c = new EntEspecialidad();
                c.Codigo = txtCodigo.Text.Trim();
                c.Nombre = txtNombre.Text.Trim();
                c.Descripcion = txtDescripcion.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogEspecialidad.Instancia.EditaEspecialidad(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            LimpiarVariables();
            listarEspecialidad();
            Deshabilitar();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código de especialidad a anular
                string codigoEspecialidad = Interaction.InputBox("Ingresa el CODIGO de Especialidad a Anular", "Deshabilitar Especialidad");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoEspecialidad))
                {
                    // Instanciar objeto para manejar la especialidad
                    EntEspecialidad c = new EntEspecialidad();
                    c.Codigo = codigoEspecialidad.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado desde el control lbEstado

                    // Llamar al método para deshabilitar la especialidad
                    LogEspecialidad.Instancia.DeshabilitarEspecialidad(c);
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
                listarEspecialidad();
                Deshabilitar();
            }

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar();
        }

        private BindingSource bindingSourceCategoria = new BindingSource();

        public void FiltrarEspecialidad(DataGridView dgvcat, string filtro)
        {
            try
            {
                DataTable dtcat = LogEspecialidad.Instancia.ObtenerEspecialidadFiltrados(filtro);
                bindingSourceCategoria.DataSource = dtcat;
                dgvcat.DataSource = bindingSourceCategoria;
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
            FiltrarEspecialidad(dgvEspecialidad, filtro);
        }

        public void CargarEspecialidadOrdenados(DataGridView dgvClientes)
        {
            try
            {
                DataTable dtBus = LogEspecialidad.Instancia.ObtenerEspecialidadOrdenado2();
                bindingSourceCategoria.DataSource = dtBus;
                dgvClientes.DataSource = bindingSourceCategoria;
                dgvClientes.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes ordenados: " + ex.Message);
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            CargarEspecialidadOrdenados(dgvEspecialidad);
        }
    }
}
