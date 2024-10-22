using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MOANSO_CAPAS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = cmbUsuario.SelectedItem.ToString();
            string password = txtContraseña.Text;

            switch (user)
            {
                case "Jefe Almacen":
                    if (password == "123")
                        OpenMenu();
                    else
                        MessageBox.Show("Contraseña incorrecta.");
                    break;
                case "Jefe Compras":
                    if (password == "456")
                        OpenMenu();
                    else
                        MessageBox.Show("Contraseña incorrecta.");
                    break;
                case "Jefe Mantenimiento":
                    if (password == "789")
                        OpenMenu();
                    else
                        MessageBox.Show("Contraseña incorrecta.");
                    break;
                default:
                    MessageBox.Show("Por favor, selecciona un usuario.");
                    break;
            }

        }

        private void OpenMenu()
        {
            string userRole = cmbUsuario.SelectedItem.ToString();

            Cargar menu = new Cargar(userRole);
            menu.Show();
            this.Hide();
        }
    }
}
