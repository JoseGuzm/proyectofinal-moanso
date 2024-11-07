using System;
using System.Collections;
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
    
    public partial class SISTEMA : Form
    {
        public int indiceImagen = 0;
        private static Button MenuActivo = null;
        private static Form FormularioActivo = null;
        private string userRole;
        public SISTEMA(string rol)
        {
            InitializeComponent();
            userRole = rol;
            Ocultar();
            ConfiguraButton();
        }

        private void ConfiguraButton()
        {
            switch (userRole)
            {
                case "Jefe Almacen":
                    button6.Visible = false;
                    panel2.Visible = false;
                    button7.Visible = false;
                    panel1.Visible = false;
                    break;
                case "Jefe Compras":
                    button8.Visible = false;
                    panel5.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void Ocultar()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel5.Visible = false;
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
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
            }
            if (panel5.Visible == true)
            {
                panel5.Visible = false;
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
            panel3.Controls.Add(formulrio);
            formulrio.Show();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Paneles();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
        
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            showSubMenu(panel1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            showSubMenu(panel2);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new Proveedor());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new Factura());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new OrdenCompra());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new ContratoMantenimiento());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            showSubMenu(panel5);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new Categoria());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new Repuestos());
            Paneles();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new IngresoRepuestos());
            Paneles();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new SalidaRepuesto());
            Paneles();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new Repuestos());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new IngresoRepuestos());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new SalidaRepuesto());
            Paneles();
            pictureBox2.Visible = false;
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new MarcaRepuesto());
            Paneles();
            pictureBox2.Visible = false;

        }

        private void button14_Click(object sender, EventArgs e)
        {
            AbrirFormularios((Button)sender, new Discrepancia());
            Paneles();
            pictureBox2.Visible = false;
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

        private void SISTEMA_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1450; // Cambiar a la frecuencia deseada
            timer1.Start();
        }
    }
}
