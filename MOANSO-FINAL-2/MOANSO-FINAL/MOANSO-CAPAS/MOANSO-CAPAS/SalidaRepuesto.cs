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
    public partial class SalidaRepuesto : Form
    {
        private LogSalidaRepuesto _logNotaSalidaRepuesto = new LogSalidaRepuesto();
        public SalidaRepuesto()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbEstado.Visible = false;
            Codigo();
            cmbCodigoBus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Codigo()
        {
            try
            {
                string codigoOTI = lbPedido.Text;
                // Obtener los códigos de los buses desde la Capa Lógica
                List<string> codigosBus = _logNotaSalidaRepuesto.ObtenerCodigosBus(codigoOTI);

                // Cargar el ComboBox con los códigos obtenidos
                cmbCodigoBus.DataSource = codigosBus;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en el ComboBox: " + ex.Message);
            }
        }

        public void Numero2()
        {
            LogEvaluacionInterna ventaBLL = new LogEvaluacionInterna();
            string numBoleta = ventaBLL.GenerarNumeroBoleta2();

            lbPedido.Text = "OP 000" + numBoleta;
        }

        public void Numero()
        {
            LogEvaluacionInterna ventaBLL = new LogEvaluacionInterna();
            string numBoleta = ventaBLL.GenerarNumeroBoleta();

            lbSalida.Text = "SR" + numBoleta;
        }


        public void Cajas(Boolean estado)
        {
            cmbCodigoBus.Enabled = estado;
            txtMarca.Enabled = estado;
            txtModelo.Enabled = estado;
            txtPlaca.Enabled = estado;
            txtCapacidad.Enabled = estado;
            txtCombustible.Enabled = estado;
            txtChasis.Enabled = estado;
            txtCodigoR.Enabled = estado;    
            txtNombreR.Enabled = estado;
            txtCategoriaR.Enabled = estado;
            txtMarcaR.Enabled = estado;
            txtEnviadaR.Enabled = estado;
            txtSolicitadaR.Enabled = estado;
            btnRegistrar.Enabled = estado;
            btnAnular.Enabled = estado;
            btnAgregar.Enabled = estado;
            btnBorrar.Enabled = estado;
        }

        private void LimpiarVariables()
        {
            cmbCodigoBus.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtPlaca.Text = "";
            txtCapacidad.Text = "";
            txtCombustible.Text = "";
            txtChasis.Text = "";
            txtCodigoR.Text = "";
            txtNombreR.Text = "";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = "";
            txtEnviadaR.Text = "";
            txtSolicitadaR.Text = "";
        }

        private void LimpiarVariables2()
        {
            txtCodigoR.Text = "";
            txtNombreR.Text = " ";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = " ";
            txtSolicitadaR.Text = "";
            txtEnviadaR.Text = "";
        }

        public void insertar()
        {
            try
            {
                string EICodigo = lbSalida.Text;
                foreach (DataGridViewRow row in dgvRepuestos.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntSalidaRepuesto detalle = new EntSalidaRepuesto()
                    {
                        DSRCodigo = EICodigo,  // Usamos el código obtenido del TextBox (ya convertido a int)
                        CodigoRepu = row.Cells["CodigoRepu"].Value.ToString(),
                        CantidadRecibida = Convert.ToInt32(row.Cells["CantidadRecibida"].Value),
                        CantidadEnviada = Convert.ToInt32(row.Cells["CantidadEnviada"].Value),
                    };
                    LogSalidaRepuesto.Instancia.InsertaDetalleSalidaRepuesto(detalle);
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
                EntSalidaRepuesto c = new EntSalidaRepuesto();
                c.CodigoSR = lbSalida.Text.Trim();
                c.BusSR = cmbCodigoBus.Text.Trim();
                c.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                c.OPCodigo = lbPedido.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogSalidaRepuesto.Instancia.InsertaSalidaRepuesto(c);
                insertar();

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
                // Solicitar al usuario el código de Salida de Repuesto a anular
                string razon = Interaction.InputBox("Ingresa el CODIGO de Salida de Repuesto a Anular", "Deshabilitar Salida de Repuesto");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(razon))
                {
                    // Instanciar objeto para manejar la Salida de Repuesto
                    EntSalidaRepuesto c = new EntSalidaRepuesto();
                    c.CodigoSR = razon.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Asignar estado (si es válido)

                    // Llamar al método para deshabilitar la Salida de Repuesto
                    LogSalidaRepuesto.Instancia.DeshabilitarSalidaRepuesto(c);
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
            string solicitado = txtSolicitadaR.Text;
            string aceptado = txtEnviadaR.Text;
            dgvRepuestos.Rows.Add(codigo, nombre, categoria, marca, solicitado, aceptado);
            LimpiarVariables2();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Cajas(true);
            Numero2();
            Numero();
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
                    EntBus bus = _logNotaSalidaRepuesto.ObtenerDatosBus(codigoBus);

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
