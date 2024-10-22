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
    public partial class ContratoMantenimiento : Form
    {
        private LogContrato _logContratoMantenimiento = new LogContrato();
        public ContratoMantenimiento()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Contar();
            codigo();
            cmbCodigoBus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCodigoProve.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void codigo()
        {
            try
            {
                // Obtener los códigos de buses y proveedores desde la Capa Lógica
                List<string> codigosBuses = _logContratoMantenimiento.ObtenerCodigosBuses();
                List<string> codigosProveedores = _logContratoMantenimiento.ObtenerCodigosProveedores();

                // Cargar los ComboBox con los códigos obtenidos
                cmbCodigoBus.DataSource = codigosBuses;
                cmbCodigoProve.DataSource = codigosProveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en los ComboBox: " + ex.Message);
            }
        }

        public void Numero()
        {
            LogContrato ventaBLL = new LogContrato(); // Instanciamos la capa lógica
            string numBoleta = ventaBLL.GenerarNumeroBoleta(); // Llamamos al método de la capa lógica

            // Asignamos el número de boleta al control Label
            lbNumero.Text = "CM" + numBoleta;
        }

        public void Contar()
        {
            int totalRegistros = 0;
            EntContrato bus = new EntContrato();  // Si es necesario crear un objeto EntBus, aunque no lo usemos en la lógica
            LogContrato negocio = new LogContrato();  // Instanciamos la capa lógica

            // Llamamos al método de la capa lógica que llama a la capa de datos
            Boolean exito = negocio.ContarTotalContrato(bus, ref totalRegistros);

            if (exito)
            {
                // Si la operación fue exitosa, mostramos el total en un Label
                lbContrato.Text = $"{totalRegistros}";
            }
        }

        public void Cajas(Boolean estado)
        {
            cmbCodigoProve.Enabled = estado;
            txtNombre.Enabled = estado;
            txtRUC.Enabled = estado;
            txtRazonSocial.Enabled = estado;
            txtTipo.Enabled = estado;
            txtTelefono.Enabled = estado;
            txtDescripcion.Enabled = estado;
            cmbCodigoBus.Enabled = estado;
            txtMarca.Enabled = estado;
            txtModelo.Enabled = estado;
            txtChasis.Enabled = estado;
            txtPlaca.Enabled = estado;
            txtCombustible.Enabled = estado;
            txtCapacidad.Enabled = estado;
            txtCosto.Enabled = estado;
            btnAnular.Enabled = estado;
            btnRegistrar.Enabled = estado;
        }

        private void LimpiarVariables()
        {
            cmbCodigoProve.Text = "";
            txtNombre.Text = " ";
            txtRUC.Text = "";
            txtRazonSocial.Text = " ";
            txtTipo.Text = "";
            txtTelefono.Text = " ";
            txtDescripcion.Text = "";
            cmbCodigoBus.Text = " ";
            txtMarca.Text = "";
            txtModelo.Text = " ";
            txtPlaca.Text = "";
            txtCombustible.Text = " ";
            txtCapacidad.Text = "";
            txtCosto.Text = "";
            txtChasis.Text = "";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntContrato c = new EntContrato();
                c.Codigo = lbNumero.Text.Trim();
                c.Bus = cmbCodigoBus.Text.Trim();
                c.Fecha = lbFecha.Text.Trim();
                c.Proveedor = cmbCodigoProve.Text.Trim();
                c.Descripcion = txtDescripcion.Text.Trim();
                c.Costo = Double.Parse(txtCosto.Text.Trim());
                c.Estado = lbEstado.Text.Trim();
                LogContrato.Instancia.InsertaContrato(c);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            Cajas(false);
            LimpiarVariables();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código de contrato a anular
                string codigoContrato = Interaction.InputBox("Ingresa el CODIGO de Contrato a Anular", "Deshabilitar Contrato");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoContrato))
                {
                    // Instanciar objeto para manejar el contrato
                    EntContrato c = new EntContrato();
                    c.Codigo = codigoContrato.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado desde el control lbEstado

                    // Llamar al método para deshabilitar el contrato
                    LogContrato.Instancia.DeshabilitarContrato(c);
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

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Numero();
            Cajas(true);
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
                    EntProveedor bus = _logContratoMantenimiento.ObtenerDatosProve(codigoBus);

                    if (bus != null)
                    {
                        // Llenar los TextBox con los datos del bus
                        txtNombre.Text = bus.Nombre;
                        txtRUC.Text = bus.RUC;
                        txtRazonSocial.Text = bus.Razon;
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

        private void cmbCodigoBus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Obtener el código del bus seleccionado
                    string codigoBus = cmbCodigoBus.SelectedItem.ToString();

                    // Obtener los datos del bus desde la capa lógica
                    EntBus bus = _logContratoMantenimiento.ObtenerDatosBus(codigoBus);

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
    }
}
