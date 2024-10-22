using CapaLogica;
using CapaEntidad;
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
    public partial class MostrarFactura : Form
    {
        private int _codigoFactura;
        public MostrarFactura(int codigoFactura)
        {
            InitializeComponent();
            _codigoFactura = codigoFactura;
            CargarFacturaDetalles(_codigoFactura);
            CargarDetallesRepuestos(_codigoFactura);
        }

        public double SumaTotal()
        {
            // Obtener el número de filas de la tabla
            int contar = dgvRepuestos.RowCount;
            double suma = 0;

            // Recorrer las filas de la tabla y sumar los valores de la columna 3
            for (int i = 0; i < contar; i++)
            {
                // Obtener el valor de la celda y convertirlo a double
                suma += Convert.ToDouble(dgvRepuestos.Rows[i].Cells[5].Value);
            }

            return suma;
        }

        private void CargarFacturaDetalles(int codigoFactura)
        {
            try
            {
                // Llamamos a la capa lógica para obtener los detalles de la factura
                LogFactura facturaLogica = new LogFactura();
                EntFactura factura = facturaLogica.ObtenerFacturaDetalles(codigoFactura);

                // Asignamos los detalles de la factura a los controles del formulario
                if (factura != null)
                {
                    lbNombreP.Text = factura.NombreEmpresa.ToString();
                    lbRazonP.Text = factura.RazonSocial.ToString();
                    lbDireccionP.Text = factura.Direccion;
                    lbTelefonoP.Text = factura.Telefono;
                    lbRUCP.Text = factura.RUC;
                    lbFechaOC.Text = factura.Fecha.ToString("dd/MM/yyyy");
                    lbFormaPagoOC.Text = factura.FormaPago;
                    lbNumeroOC.Text = factura.CodigoFactura.ToString();
                }
                else
                {
                    MessageBox.Show("No se encontraron detalles para la factura.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los detalles de la factura: " + ex.Message);
            }
        }

        private void CargarDetallesRepuestos(int codigoFactura)
        {
            try
            {
                // Crear una instancia de la Capa Lógica
                LogFactura facturaLogica = new LogFactura();

                // Obtener los detalles de los repuestos
                List<EntFactura> detallesRepuesto = facturaLogica.ObtenerDetallesRepuesto(codigoFactura);

                // Asignar los detalles al DataGridView
                dgvRepuestos.DataSource = detallesRepuesto;

                // Si necesitas configurar las columnas de manera personalizada
                // (Por ejemplo, renombrar las cabeceras)
                dgvRepuestos.Columns[0].HeaderText = "Descripción";
                dgvRepuestos.Columns[1].HeaderText = "Categoría";
                dgvRepuestos.Columns[2].HeaderText = "Marca";
                dgvRepuestos.Columns[3].HeaderText = "Cantidad";
                dgvRepuestos.Columns[4].HeaderText = "Precio";

                foreach (DataGridViewRow row in dgvRepuestos.Rows)
                {
                    // Asegurarse de que la fila no sea una fila nueva o vacía
                    if (!row.IsNewRow)
                    {
                        // Obtener los valores de las columnas que deseas multiplicar (asegúrate de que sean numéricos)
                        double num1 = Convert.ToDouble(row.Cells[3].Value);  // Suponiendo que la primera columna es la 1
                        double num2 = Convert.ToDouble(row.Cells[4].Value);  // Suponiendo que la segunda columna es la 2

                        // Realizar la multiplicación
                        double resultado = num1 * num2;

                        // Colocar el resultado en la tercera columna (cambia el índice según sea necesario)
                        row.Cells[5].Value = resultado;  // Suponiendo que la tercera columna es la 3
                    }
                }

                double IGV = SumaTotal() * 0.18;
                double total = SumaTotal() + IGV;
                lbImporte.Text = SumaTotal().ToString();
                lbIGV.Text = IGV.ToString();
                lbTotal.Text = total.ToString();

                /*
                lbImporte.Text = SumaTotal().ToString();
                double IGV = int.Parse(lbImporte.Text);
                double IGV2 = IGV * (0.18 * IGV);
                lbIGV.Text = IGV2.ToString();
                double total = IGV + IGV2;
                lbTotal.Text = total.ToString();
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los detalles de la factura: " + ex.Message);
            }
        }
    }
}
