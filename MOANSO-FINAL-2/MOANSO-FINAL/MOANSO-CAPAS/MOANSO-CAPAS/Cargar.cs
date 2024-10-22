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
    public partial class Cargar : Form
    {
        private string userRole;
        public Cargar(string rol)
        {
            InitializeComponent();
            userRole = rol;
            ConfiguraLabel();
        }
        //int cont = 0;

        private void ConfiguraLabel()
        {
            switch (userRole)
            {
                case "Jefe Almacen":
                    label2.Text = "JEFE ALMACEN";
                    break;
                case "Jefe Compras":
                    label2.Text = "JEFE COMPRAS";
                    break;
                case "Jefe Mantenimiento":
                    label2.Text = "JEFE MANTENIMIENTO";
                    break;
                default:
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.Opacity < 1) this.Opacity += 0.05;
            circularProgressBar1.Value += 1;
            circularProgressBar1.Text = circularProgressBar1.Value.ToString();
            if(circularProgressBar1.Value == 100)
            {
                timer1.Stop();
                timer2.Start();
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.03;
            if (this.Opacity == 0)
            {
                timer2.Stop();
                this.Close();


                if(userRole == "Jefe Almacen" || userRole == "Jefe Compras")
                {
                    SISTEMA menuForm = new SISTEMA(userRole);
                    menuForm.Show();
                    this.Hide();
                }
                if (userRole == "Jefe Mantenimiento")
                {
                    JefeMantenimiento menuForm2 = new JefeMantenimiento();
                    menuForm2.Show();
                    this.Hide();
                }
            }
        }

        private void Cargar_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.0;
            circularProgressBar1.Value = 0;
            circularProgressBar1.Minimum = 0;
            circularProgressBar1.Maximum = 100;
            timer1.Start();

            
        }
    }
}
