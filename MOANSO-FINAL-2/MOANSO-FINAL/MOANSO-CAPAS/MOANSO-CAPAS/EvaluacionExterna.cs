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
    public partial class EvaluacionExterna : Form
    {
        private LogEvaluacionExterna _logEvaluacionExterna = new LogEvaluacionExterna();

        public EvaluacionExterna()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbEstado.Visible = false;
            Codigo();
            cmbCodigoProve.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCodigoBus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Codigo()
        {
            try
            {
                string codigoTE = lbOTE.Text;
                // Obtener los códigos de buses y proveedores desde la Capa Lógica
                List<string> codigosBuses = _logEvaluacionExterna.ObtenerCodigosBuses(codigoTE);
                List<string> codigosProveedores = _logEvaluacionExterna.ObtenerCodigosProveedores(codigoTE);

                // Cargar los ComboBox con los códigos obtenidos
                cmbCodigoBus.DataSource = codigosBuses;
                cmbCodigoProve.DataSource = codigosProveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en los ComboBox: " + ex.Message);
            }
        }

        public void Numero2()
        {
            LogEvaluacionExterna ventaBLL = new LogEvaluacionExterna(); // Instanciamos la capa lógica
            string numBoleta = ventaBLL.GenerarNumeroBoleta2(); // Llamamos al método de la capa lógica

            // Asignamos el número de boleta al control Label
            lbOTE.Text = "OTE 000" + numBoleta;

        }        

        public void Numero()
        {
            LogEvaluacionExterna ventaBLL = new LogEvaluacionExterna(); // Instanciamos la capa lógica
            string numBoleta = ventaBLL.GenerarNumeroBoleta(); // Llamamos al método de la capa lógica

            // Asignamos el número de boleta al control Label
            lbEvaluacion.Text = "EME "+ numBoleta;
        }


        public void Contar()
        {
            int totalRegistros = 0;
            EntEvaluacionExterna bus = new EntEvaluacionExterna();  // Si es necesario crear un objeto EntBus, aunque no lo usemos en la lógica
            LogEvaluacionExterna negocio = new LogEvaluacionExterna();  // Instanciamos la capa lógica

            // Llamamos al método de la capa lógica que llama a la capa de datos
            Boolean exito = negocio.ContarTotalEvaluacionExterna(bus, ref totalRegistros);

            if (exito)
            {
                // Si la operación fue exitosa, mostramos el total en un Label
                lbEvaluacion.Text = $"{totalRegistros}";
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
            cmbCodigoBus.Enabled = estado;
            txtMarca.Enabled = estado;
            txtModelo.Enabled = estado;
            txtChasis.Enabled = estado;
            txtPlaca.Enabled = estado;
            txtCombustible.Enabled = estado;
            txtCapacidad.Enabled = estado;
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
            cmbCodigoBus.Text = " ";
            txtMarca.Text = "";
            txtModelo.Text = " ";
            txtPlaca.Text = "";
            txtCombustible.Text = " ";
            txtCapacidad.Text = "";
            txtChasis.Text = "";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntEvaluacionExterna c = new EntEvaluacionExterna();
                c.CodigoEE = lbEvaluacion.Text.Trim();
                c.CodigoBus = cmbCodigoBus.Text.Trim();
                c.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                c.ProveedorEE = cmbCodigoProve.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                c.TECodigo = lbOTE.Text.Trim();
                LogEvaluacionExterna.Instancia.InsertaEvaluacionExterna(c);

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
                // Solicitar al usuario el código de evaluación externa a anular
                string codigoEvaluacionExterna = Interaction.InputBox("Ingresa el CODIGO de Evaluación Externa a Anular", "Deshabilitar Evaluación Externa");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoEvaluacionExterna))
                {
                    // Instanciar objeto para manejar la evaluación externa
                    EntEvaluacionExterna c = new EntEvaluacionExterna();
                    c.CodigoEE = codigoEvaluacionExterna.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado desde el control lbEstado

                    // Llamar al método para deshabilitar la evaluación externa
                    LogEvaluacionExterna.Instancia.DeshabilitarEvaluacionExterna2(c);
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
            Numero2();
            Cajas(true);
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
                    EntBus bus = _logEvaluacionExterna.ObtenerDatosBus(codigoBus);

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

        private void cmbCodigoProve_MouseDown(object sender, MouseEventArgs e)
        {
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
                    EntProveedor bus = _logEvaluacionExterna.ObtenerDatosProve(codigoBus);

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
    }
}
