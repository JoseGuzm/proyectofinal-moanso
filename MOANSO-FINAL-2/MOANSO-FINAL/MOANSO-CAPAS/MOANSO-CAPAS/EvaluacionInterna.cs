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
    public partial class EvaluacionInterna : Form
    {
        private LogEvaluacionInterna _logEvaluacionInterna = new LogEvaluacionInterna();
        public EvaluacionInterna()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbEstado.Visible = false;
            Codigo();
            cmbCodigoBus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCodigoMeca.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Codigo()
        {
            try
            {
                string codigoOTI = lbOTI.Text;
                // Obtener los códigos de buses y mecánicos desde la Capa Lógica
                List<string> codigosBuses = _logEvaluacionInterna.ObtenerCodigosBuses(codigoOTI);
                List<string> codigosMecanicos = _logEvaluacionInterna.ObtenerCodigosMecanicos(codigoOTI);

                // Cargar los ComboBox con los códigos obtenidos
                cmbCodigoBus.DataSource = codigosBuses;
                cmbCodigoMeca.DataSource = codigosMecanicos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en los ComboBox: " + ex.Message);
            }
        }

        public void Numero2()
        {
            LogEvaluacionInterna ventaBLL = new LogEvaluacionInterna(); // Instanciamos la capa lógica
            string numBoleta = ventaBLL.GenerarNumeroBoleta2(); // Llamamos al método de la capa lógica

            // Asignamos el número de boleta al control Label
            lbOTI.Text = "OTI 000" + numBoleta;
        }


        public void Numero()
        {
            LogEvaluacionInterna ventaBLL = new LogEvaluacionInterna(); // Instanciamos la capa lógica
            string numBoleta = ventaBLL.GenerarNumeroBoleta(); // Llamamos al método de la capa lógica

            // Asignamos el número de boleta al control Label
            lbEvaluacion.Text = "EMI" + numBoleta;
        }

        

        public void Cajas(Boolean estado)
        {
            cmbCodigoBus.Enabled = estado;
            txtPlaca.Enabled = estado;
            txtMarca.Enabled = estado;
            txtModelo.Enabled = estado;
            txtChasis.Enabled = estado;
            txtCapacidad.Enabled = estado;
            cmbCodigoMeca.Enabled = estado;
            txtNombre.Enabled = estado;
            txtCargo.Enabled = estado;
            txtDNI.Enabled = estado;
            txtTelefono.Enabled = estado;
            btnAnular.Enabled = estado;
            btnRegistrar.Enabled = estado;
        }

        private void LimpiarVariables()
        {
            cmbCodigoBus.Text = "";
            txtPlaca.Text = " ";
            txtMarca.Text = "";
            txtModelo.Text = " ";
            txtChasis.Text = "";
            txtCapacidad.Text = " ";
            cmbCodigoMeca.Text = "";
            txtNombre.Text = " ";
            txtCargo.Text = "";
            txtDNI.Text = " ";
            txtTelefono.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string codigo = cmbCodigoMeca.Text;
            string nombre = txtNombre.Text;
            string cargo = txtCargo.Text;
            string dni = txtDNI.Text;
            string telefono = txtTelefono.Text;
            dgvMecanico.Rows.Add(codigo, nombre, cargo, dni, telefono);
        }

        public void insertar()
        {
            try
            { 
                string EICodigo = lbEvaluacion.Text;
                foreach (DataGridViewRow row in dgvMecanico.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntEvaluacionInterna detalle = new EntEvaluacionInterna()
                    {
                        EICodigo = EICodigo,  // Usamos el código obtenido del TextBox (ya convertido a int)
                        MecanicoEI = row.Cells["MecanicoEI"].Value.ToString()   // Obtener el código del mecánico de la fila
                    };
                    LogEvaluacionInterna.Instancia.InsertaDetalleEvaluacionInterna(detalle);
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
                EntEvaluacionInterna c = new EntEvaluacionInterna();
                c.CodigoEI = lbEvaluacion.Text.Trim();
                c.CodigoBus = cmbCodigoBus.Text.Trim();
                c.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                c.TICodigo = lbOTI.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogEvaluacionInterna.Instancia.InsertaEvaluacionInterna(c);
                insertar();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error.." + ex);
            }
            LimpiarVariables();
            Cajas(false);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Cajas(true);
            Numero2();
            Numero();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar al usuario el código de evaluación interna a anular
                string codigoEvaluacionInterna = Interaction.InputBox("Ingresa el CODIGO de Evaluación Interna a Anular", "Deshabilitar Evaluación Interna");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoEvaluacionInterna))
                {
                    // Instanciar objeto para manejar la evaluación interna
                    EntEvaluacionInterna c = new EntEvaluacionInterna();
                    c.CodigoEI = codigoEvaluacionInterna.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado desde el control lbEstado

                    // Llamar al método para deshabilitar la evaluación interna
                    LogEvaluacionInterna.Instancia.DeshabilitarEvaluacionInterna(c);
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

        private void cmbCodigoBus_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Obtener el código del bus seleccionado
                    string codigoBus = cmbCodigoBus.SelectedItem.ToString();

                    // Obtener los datos del bus desde la capa lógica
                    EntBus bus = _logEvaluacionInterna.ObtenerDatosBus(codigoBus);

                    if (bus != null)
                    {
                        // Llenar los TextBox con los datos del bus
                        txtMarca.Text = bus.Marca;
                        txtModelo.Text = bus.Modelo;
                        txtPlaca.Text = bus.NPlaca;
                        txtChasis.Text = bus.NChasis;
                        //txtCombustible.Text = bus.Combustible;
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

        private void cmbCodigoMeca_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Obtener el código del bus seleccionado
                    string codigoBus = cmbCodigoMeca.SelectedItem.ToString();

                    // Obtener los datos del bus desde la capa lógica
                    EntMecanico bus = _logEvaluacionInterna.ObtenerDatosMecani(codigoBus);

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
    }
}