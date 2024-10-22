using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MOANSO_CAPAS
{
    public partial class JefeMantenimiento : Form
    {
        public int indiceImagen = 0;
        private static Button MenuActivo = null;
        private static Form FormularioActivo = null;
        public JefeMantenimiento()
        {
            InitializeComponent();
            Ocultar();
        }

        private void Ocultar()
        {
            panel3.Visible = false;
            panel4.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                Paneles();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private void Paneles()
        {
            if (panel3.Visible == true)
            {
                panel3.Visible = false;
            }
            if (panel4.Visible == true)
            {
                panel4.Visible = false;
            }
        }

        private void AbrirFormularios(Button menu, Form formulrio)
        {
            if (MenuActivo != null)
            {
                MenuActivo.BackColor = Color.LightSkyBlue;
                MenuActivo.ForeColor = Color.Black;

            }
            menu.BackColor = Color.Black;
            menu.ForeColor = Color.White;
            MenuActivo = menu;

            if (FormularioActivo != null)
            {
                FormularioActivo.Close();
            }
            FormularioActivo = formulrio;
            formulrio.TopLevel = false;
            formulrio.Dock = DockStyle.Fill;
            panel5.Controls.Add(formulrio);
            formulrio.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showSubMenu(panel3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            showSubMenu(panel4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new Bus());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new Especialidad());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new MECANICOS());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new PedidoRepuesto());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new EvaluacionInterna());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new EvaluacionExterna());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }

        private void JefeMantenimiento_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1450; // Cambiar a la frecuencia deseada
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] imagenes = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMAGEN"));
            if (imagenes.Length > 0)
            {
                try
                {
                    if (indiceImagen >= imagenes.Length)
                    {
                        indiceImagen = 0;
                    }
                    pictureBox2.Image = Image.FromFile(imagenes[indiceImagen]);
                    indiceImagen++;
                }
                catch
                {
                    indiceImagen = 0;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new TrabajoInterno());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new TrabajoExterno());
            Paneles();
            pictureBox2.Visible = false;
        }
    }
}
