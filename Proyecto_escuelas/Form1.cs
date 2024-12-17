using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_escuelas
{
    public partial class Form1 : Form

    {
        public static class UsuarioActual
        {
            public static string Nombre { get; set; }
            public static string Rol { get; set; }
        }
        private string nombreUsuario;
        private string rolUsuario;

        public Form1(string usuario, string rol)
        {
            InitializeComponent();

            // Asignar los valores a las propiedades estáticas de UsuarioActual
            UsuarioActual.Nombre = usuario;
            UsuarioActual.Rol = rol;

            // Configurar visibilidad de botones según el rol
            if (UsuarioActual.Rol == "Admin")
            {
                button1.Visible = true;
                button7.Visible = true;
            }
            else
            {
                button1.Visible = false;
                button7.Visible = false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (UsuarioActual.Rol == "Admin")
            {
                button1.Visible = true;
                button7.Visible = true;
            }
            else
            {
                button1.Visible = false;
                button7.Visible = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
                Principal principalForm = new Principal();
                principalForm.Show();
                principalForm.tabControl1.SelectedIndex = 0;
                this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clientes clientes = new Clientes();
            clientes.Show();
            clientes.tabControl1.SelectedIndex = 1;
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Principal principalForm = new Principal();
            principalForm.Show();
            principalForm.tabControl1.SelectedIndex = 2;
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Principal principalForm = new Principal();
            principalForm.Show();
            principalForm.tabControl1.SelectedIndex = 3;
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Principal principalForm = new Principal();
            principalForm.Show();
            principalForm.tabControl1.SelectedIndex = 4;
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Inscripciones incripcion = new Inscripciones();
            incripcion.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Listado listado = new Listado();
            listado.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Agregar_o_Quitar_Stock Agregar_o_Quitar_Stock = new Agregar_o_Quitar_Stock();
            Agregar_o_Quitar_Stock.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Administracion Administracion = new Administracion();
            Administracion.Show();
            this.Hide();
        }
    }
}
