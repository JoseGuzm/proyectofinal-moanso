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
    public partial class Repuestos : Form
    {
        public Repuestos()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            /*
            DateTime fechaSeleccionada = e.Start;

            // Formatear la fecha en el formato YYYY-MM-DD
            string fechaFormateada = fechaSeleccionada.ToString("yyyy-MM-dd");

            // Asignar la fecha formateada al TextBox
            txtFecha.Text = fechaFormateada;
            */
        }
    }
}
