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
    public partial class TrabajoExterno : Form
    {
        private LogOTE _logOrdenTrabajoExterno = new LogOTE();
        public TrabajoExterno()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbEstado.Visible = false;
            listarRepuestos();
            cmbCodigoBus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCodigoProve.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParte.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPieza.DropDownStyle = ComboBoxStyle.DropDownList;
            
        }

        public void listarRepuestos()
        {
            dgvLista.DataSource = LogOTE.Instancia.ListarRepuesto2();
        }

        public void Codigo()
        {
            try
            {
                string codigoTE = lbContrato.Text;
                // Obtener los códigos de los buses desde la Capa Lógica
                List<string> codigosBus = _logOrdenTrabajoExterno.ObtenerCodigosBus(codigoTE);
                // Obtener los códigos de los proveedores desde la Capa Lógica
                List<string> codigosProveedor = _logOrdenTrabajoExterno.ObtenerCodigosProveedor(codigoTE);

                // Cargar los ComboBox con los códigos obtenidos
                cmbCodigoBus.DataSource = codigosBus;
                cmbCodigoProve.DataSource = codigosProveedor;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en los ComboBox: " + ex.Message);
            }
        }

        public void Numero2()
        {
            LogOTE ventaBLL = new LogOTE();
            string numBoleta = ventaBLL.GenerarNumeroBoleta2();

            lbContrato.Text = "CM" + numBoleta;
        }

        public void Numero()
        {
            LogOTE ventaBLL = new LogOTE();
            string numBoleta = ventaBLL.GenerarNumeroBoleta();

            lbOTE.Text = "OTE" + numBoleta;
        }


        public void Cajas(Boolean estado)
        {
            cmbCodigoBus.Enabled = estado;
            txtMarca.Enabled = estado;
            txtModelo.Enabled = estado;
            txtPlaca.Enabled = estado;
            txtChasis.Enabled = estado;
            txtCombustible.Enabled = estado;
            txtCapacidad.Enabled = estado;
            txtCodigoR.Enabled = estado;
            txtNombreR.Enabled = estado;
            txtCategoriaR.Enabled = estado;
            txtMarcaR.Enabled = estado;
            txtCantidadR.Enabled = estado;
            cmbParte.Enabled = estado;
            cmbPieza.Enabled = estado;
            cmbCodigoProve.Enabled = estado;
            txtNombre.Enabled = estado;
            txtRUC.Enabled = estado;
            txtRazon.Enabled = estado;
            txtTelefono.Enabled = estado;
            btnRegistrar.Enabled = estado;
            btnAnular.Enabled = estado;
        }

        private void LimpiarVariables()
        {
            cmbCodigoBus.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtPlaca.Text = "";
            txtChasis.Text = "";
            txtCombustible.Text = "";
            txtCapacidad.Text = "";
            txtCodigoR.Text = "";
            txtNombreR.Text = "";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = "";
            txtCantidadR.Text = "";
            cmbParte.Text = "";
            cmbPieza.Text = "";
            cmbCodigoProve.Text = "";
            txtNombre.Text = "";
            txtRUC.Text = "";
            txtRazon.Text = "";
            txtTelefono.Text = "";
        }

        public void insertar()
        {
            try
            {
                string EICodigo = lbOTE.Text;
                foreach (DataGridViewRow row in dgvRepuestos.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntOTE detalle = new EntOTE()
                    {
                        TECodigo = EICodigo,  // Usamos el código obtenido del TextBox (ya convertido a int)
                        CodigoRepu = row.Cells["Column1"].Value.ToString(),
                        Cantidad = Convert.ToInt32(row.Cells["Column5"].Value),
                    };
                    LogOTE.Instancia.InsertaDetalleOTE(detalle);
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

        public void insertar2()
        {
            try
            {;
                foreach (DataGridViewRow row in dgvDiagnostico.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntOTE detalle = new EntOTE()
                    {
                        Parte = row.Cells["Parte"].Value.ToString(),
                        Pieza = row.Cells["Pieza"].Value.ToString(),
                    };
                    LogOTE.Instancia.InsertaDetalleOTE(detalle);
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
                EntOTE c = new EntOTE();
                c.CodigoTE = lbOTE.Text.Trim();
                c.CodigoBus = cmbCodigoBus.Text.Trim();
                c.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                c.ContratoCO = lbContrato.Text.Trim();
                c.ProveedorTE = cmbCodigoProve.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogOTE.Instancia.InsertaOTE(c);
                insertar();
                insertar2();

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
                // Solicitar al usuario el código de OTE a anular
                string razon = Interaction.InputBox("Ingresa el CODIGO de OTE a Anular", "Deshabilitar Documento");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(razon))
                {
                    // Instanciar objeto para manejar la OTE
                    EntOTE c = new EntOTE();
                    c.CodigoTE = razon.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Asignar estado (si es válido)

                    // Llamar al método para deshabilitar el OTE
                    LogOTE.Instancia.DeshabilitarOTE(c);
                }
                else
                {
                    MessageBox.Show("Operación cancelada o razón no proporcionada.");
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
            string cantidad = txtCantidadR.Text;
            dgvRepuestos.Rows.Add(codigo, nombre, categoria, marca, cantidad);
        }

        private void btnAgregarM_Click(object sender, EventArgs e)
        {
            string parte = cmbParte.Text;
            string pieza = cmbPieza.Text;     
            dgvDiagnostico.Rows.Add(parte, pieza);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Cajas(true);
            Numero();
            Numero2();
            Codigo();
        }

        private void cmbCodigoBus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Obtener el código del bus seleccionado
                    string codigoBus = cmbCodigoBus.SelectedItem.ToString();

                    // Obtener los datos del bus desde la capa lógica
                    EntBus bus = _logOrdenTrabajoExterno.ObtenerDatosBus(codigoBus);

                    if (bus != null)
                    {
                        // Llenar los TextBox con los datos del bus
                        txtMarca.Text = bus.Marca;
                        txtModelo.Text = bus.Modelo;
                        txtPlaca.Text = bus.NPlaca;
                        txtChasis.Text = bus.NChasis;
                        txtCombustible.Text = bus.Combustible;
                        txtCapacidad.Text = bus.Capacidad.ToString();
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

        private void cmbCodigoProve_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Obtener el código del bus seleccionado
                    string codigoBus = cmbCodigoProve.SelectedItem.ToString();

                    // Obtener los datos del bus desde la capa lógica
                    EntProveedor bus = _logOrdenTrabajoExterno.ObtenerDatosProve(codigoBus);

                    if (bus != null)
                    {
                        // Llenar los TextBox con los datos del bus
                        txtNombre.Text = bus.Nombre;
                        txtRUC.Text = bus.RUC;
                        txtRazon.Text = bus.Razon;
                        //txtTipo.Text = bus.Tipo;
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

        private void dgvLista_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow filaActual = dgvLista.Rows[e.RowIndex];
            txtCodigoR.Text = filaActual.Cells[0].Value.ToString();
            txtNombreR.Text = filaActual.Cells[1].Value.ToString();
            txtCategoriaR.Text = filaActual.Cells[2].Value.ToString();
            txtMarcaR.Text = filaActual.Cells[3].Value.ToString();
            //txtCantidadR.Text = filaActual.Cells[7].Value.ToString();
        }
    }
}
