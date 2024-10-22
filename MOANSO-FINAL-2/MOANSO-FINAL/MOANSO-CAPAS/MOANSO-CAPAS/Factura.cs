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

namespace MOANSO_CAPAS
{
    public partial class Factura : Form
    {
        public Factura()
        {
            InitializeComponent();
            Contar();
            listarBus();
        }

        public void Contar()
        {
            int totalRegistros = 0;
            EntFactura bus = new EntFactura();  
            LogFactura negocio = new LogFactura(); 

            Boolean exito = negocio.ContarTotalFactura(bus, ref totalRegistros);

            if (exito)
            {
                lbFactura.Text = $"{totalRegistros}";
            }
        }

        public void listarBus()
        {
            dgvFacturas.DataSource = LogFactura.Instancia.ListarFactura2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Verificar si hay una fila seleccionada en el DataGridView
            if (dgvFacturas.SelectedRows.Count > 0)
            {
                // Obtener el valor de la primera celda de la fila seleccionada (CódigoFactura) y convertirlo a int
                int codigoFacturaSeleccionado = Convert.ToInt32(dgvFacturas.SelectedRows[0].Cells[0].Value);

                // Crear una instancia del formulario MostrarFactura y pasarle el CodigoFactura seleccionado
                MostrarFactura mostrarFacturaForm = new MostrarFactura(codigoFacturaSeleccionado);

                // Mostrar el formulario MostrarFactura
                mostrarFacturaForm.Show();

                // Opcional: Ocultar el formulario actual (Factura) si lo deseas
                this.Hide();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila en el DataGridView.");
            }
        }
    }
}