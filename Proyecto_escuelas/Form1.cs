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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            Principal principalForm = new Principal();
            principalForm.Show();
            principalForm.tabControl1.SelectedIndex = 1;
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
            Listados listado = 
        }
    }
}
