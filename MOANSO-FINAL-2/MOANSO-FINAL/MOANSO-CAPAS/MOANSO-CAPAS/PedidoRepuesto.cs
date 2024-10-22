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
    public partial class PedidoRepuesto : Form
    {
        public PedidoRepuesto()
        {
            InitializeComponent();
            Cajas(false);
            lbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbEstado.Visible = false;
            cmbJefe.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Numero2()
        {
            LogOrdenPedido ventaBLL = new LogOrdenPedido();
            string numBoleta = ventaBLL.GenerarNumeroBoleta2();

            lbOTI.Text = "OTI 000" + numBoleta;
        }

        public void Numero()
        {
            LogOrdenPedido ventaBLL = new LogOrdenPedido();
            string numBoleta = ventaBLL.GenerarNumeroBoleta();

            lbPedido.Text = "OP" + numBoleta;
        }


        public void Cajas(Boolean estado)
        {
            cmbJefe.Enabled = estado;
            txtDescripcion.Enabled = estado;
            txtCodigoR.Enabled = estado;
            txtNombreR.Enabled = estado;
            txtCategoriaR.Enabled = estado;
            txtMarcaR.Enabled = estado;
            txtPrecioR.Enabled = estado;
            txtCantidadR.Enabled = estado;
            btnRegistrar.Enabled = estado;
            btnAnular.Enabled = estado;
        }

        private void LimpiarVariables()
        {
            cmbJefe.Text = "";
            txtDescripcion.Text = " ";
            txtCodigoR.Text = "";
            txtNombreR.Text = " ";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = " ";
            txtPrecioR.Text = "";
            txtCantidadR.Text = "";
        }

        private void LimpiarVariables2()
        {
            txtCodigoR.Text = "";
            txtNombreR.Text = " ";
            txtCategoriaR.Text = "";
            txtMarcaR.Text = " ";
            txtPrecioR.Text = "";
            txtCantidadR.Text = "";
        }

        public void insertar()
        {
            try
            {
                string EICodigo = lbPedido.Text;
                foreach (DataGridViewRow row in dgvRepuestos.Rows)
                {
                    if (row.IsNewRow) continue;
                    EntOrdenPedido detalle = new EntOrdenPedido()
                    {
                        OPCodigo = EICodigo,  // Usamos el código obtenido del TextBox (ya convertido a int)
                        CodigoRepu = row.Cells["CodigoRepu"].Value.ToString(),
                        Cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value),
                    };
                    LogOrdenPedido.Instancia.InsertaDetalleOrdenPedido(detalle);
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

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Cajas(true);
            Numero();
            Numero2();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                EntOrdenPedido c = new EntOrdenPedido();
                c.CodigoOP = lbPedido.Text.Trim();
                c.TICodigo = lbOTI.Text.Trim();
                c.Fecha = DateTime.Parse(lbFecha.Text.Trim());
                c.JefeEncargado = cmbJefe.Text.Trim();
                c.Descripcion = txtDescripcion.Text.Trim();
                c.Estado = lbEstado.Text.Trim();
                LogOrdenPedido.Instancia.InsertaOrdenPedido(c);
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
                // Solicitar al usuario el código de la orden de pedido a anular
                string codigoOP = Interaction.InputBox("Ingresa el CODIGO de Orden de Pedido a Anular", "Deshabilitar Orden de Pedido");

                // Verificar que el código ingresado no sea vacío
                if (!string.IsNullOrEmpty(codigoOP))
                {
                    // Instanciar objeto para manejar la orden de pedido
                    EntOrdenPedido c = new EntOrdenPedido();
                    c.CodigoOP = codigoOP.Trim(); // Asignar el código ingresado por el usuario
                    c.Estado = lbEstado.Text.Trim(); // Obtener el estado de la orden desde el control lbEstado

                    // Llamar al método para deshabilitar la orden de pedido
                    LogOrdenPedido.Instancia.DeshabilitarOrdenPedido(c);
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
                Cajas(false);
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigoR.Text;
            string nombre = txtNombreR.Text;
            string categoria = txtCategoriaR.Text;
            string marca = txtMarcaR.Text;
            string precio = txtPrecioR.Text;
            string cantidad = txtCantidadR.Text;
            dgvRepuestos.Rows.Add(codigo, nombre, categoria, marca, precio, cantidad);
            LimpiarVariables2();
        }
    }
}
