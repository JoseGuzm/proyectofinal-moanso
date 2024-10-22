using CapaEntidad;
using CapaLogica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace MOANSO_CAPAS
{
    public partial class TrabajoInterno : Form
    {
        private LogOTI _logOrdenTrabajoInterno = new LogOTI();

        public TrabajoInterno()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbEstado.Visible = false;
            Codigo();
            cmbCodigoBus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCodigoMecanico.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParte.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPieza.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Codigo()
        {
            try
            {
                // Obtener los códigos de los buses desde la Capa Lógica
                List<string> codigosBus = _logOrdenTrabajoInterno.ObtenerCodigosBus();
                // Obtener los códigos de los mecánicos desde la Capa Lógica
                List<string> codigosMecanico = _logOrdenTrabajoInterno.ObtenerCodigosMecanico();

                    // Cargar los ComboBox con los códigos obtenidos
                cmbCodigoBus.DataSource = codigosBus;
                cmbCodigoMecanico.DataSource = codigosMecanico;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en los ComboBox: " + ex.Message);
            }
        }    

        public void Numero()
        {
            LogOTI ventaBLL = new LogOTI();
            string numBoleta = ventaBLL.GenerarNumeroBoleta();

            lbOTI.Text = "OTI" + numBoleta;
        }


        public void Cajas(Boolean estado)
        {
            cmbCodigoMecanico.Enabled = estado;
            txtNombre.Enabled = estado;
            txtCargo.Enabled = estado;
            txtDNI.Enabled = estado;
            txtTelefono.Enabled = estado;
            cmbCodigoBus.Enabled = estado;
            txtMarca.Enabled = estado;
            txtModelo.Enabled = estado;
            txtPlaca.Enabled = estado;
            txtChasis.Enabled = estado;
            cmbParte.Enabled = estado;
            cmbPieza.Enabled = estado;
            txtCodigoR.Enabled = estado;
            txtNombreR.Enabled = estado;
            txtCategoriaR.Enabled = estado;
            txtMarcaR.Enabled = estado;
            txtCantidadR.Enabled = estado;
            txtTelefono.Enabled = estado;
            btnRegistrar.Enabled = estado;
            btnAnular.Enabled = estado;
        }

        private void LimpiarVariables()
        {
            cmbCodigoMecanico.Text = "";
            txtNombre.Text = "";
            txtCargo.Text = "";
            txtDNI.Text = "";
            txtTelefono.Text = "";
            cmbCodigoBus.Text = ""; 
            txtModelo.Text = "";
            txtPlaca.Text = "";
            txtChasis.Text = "";
            cmbParte.Text = "";
            cmbPieza.Text = "";
            txtCodigoR.Text = "";
            txtNombreR.Text = "";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = "";
            txtCantidadR.Text = "";
            txtTelefono.Text = "";
        }

        public void insertar()
        {
            try
            {
                string EICodigo = lbOTI.Text;
                foreach (DataGridViewRow row in dgvRepuestos.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntOTI detalle = new EntOTI()
                    {
                        OrdentrabajointernoID = EICodigo,  // Usamos el código obtenido del TextBox (ya convertido a int)
                        CodigoRepu = row.Cells["CodigoRepu"].Value.ToString(),
                        Cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value),
                    };
                    LogOTI.Instancia.InsertaDetalleOTI(detalle);
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
            {
                ;
                foreach (DataGridViewRow row in dgvDiagnostico.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntOTI detalle = new EntOTI()
                    {
                        Parte = row.Cells["Parte"].Value.ToString(),
                        Pieza = row.Cells["Pieza"].Value.ToString(),
                    };
                    LogOTI.Instancia.InsertaDetalleOTI(detalle);
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

        public void insertar3()
        {
            try
            {
                ;
                foreach (DataGridViewRow row in dgvMecanicos.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntOTI detalle = new EntOTI()
                    {
                        MecanicoTI = row.Cells["MecanicoTI"].Value.ToString(),
                    };
                    LogOTI.Instancia.InsertaDetalleOTI(detalle);
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
                EntOTI c = new EntOTI();
                c.CodigoTI = lbOTI.Text.Trim();
                c.BusTI = cmbCodigoBus.Text.Trim();
                c.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                c.Estado = lbEstado.Text.Trim();
                LogOTI.Instancia.InsertaOTI(c);
                insertar();
                insertar2();
                insertar3();

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
                string razon = Interaction.InputBox("¿Ingresa CODIGO de OTI a Anular?", "Deshabilitar Documento");
                if (!string.IsNullOrEmpty(razon))
                {
                    EntOTI c = new EntOTI();
                    c.CodigoTI = lbOTI.Text.Trim();
                    c.Estado = lbEstado.Text.Trim();
                    LogOTI.Instancia.DeshabilitarOTI(c);
                }
                else
                {
                    MessageBox.Show("Operación cancelada o razón no proporcionada.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            LimpiarVariables();
            Cajas(false);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string razon = Interaction.InputBox("¿Por qué deseas deshabilitar este documento?", "Deshabilitar Documento");
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Cajas(true);
            Numero();
        }

        private void cmbCodigoMecanico_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Obtener el código del bus seleccionado
                    string codigoBus = cmbCodigoMecanico.SelectedItem.ToString();

                    // Obtener los datos del bus desde la capa lógica
                    EntMecanico bus = _logOrdenTrabajoInterno.ObtenerDatosMecani(codigoBus);

                    if (bus != null)
                    {
                        // Llenar los TextBox con los datos del bus
                        txtNombre.Text = bus.Nombre;
                        txtCargo.Text = bus.Especialidad;
                        txtDNI.Text = bus.DNI;
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

        private void cmbCodigoBus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Obtener el código del bus seleccionado
                    string codigoBus = cmbCodigoBus.SelectedItem.ToString();

                    // Obtener los datos del bus desde la capa lógica
                    EntBus bus = _logOrdenTrabajoInterno.ObtenerDatosBus(codigoBus);

                    if (bus != null)
                    {
                        // Llenar los TextBox con los datos del bus
                        txtMarca.Text = bus.Marca;
                        txtModelo.Text = bus.Modelo;
                        txtPlaca.Text = bus.NPlaca;
                        txtChasis.Text = bus.NChasis;
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
