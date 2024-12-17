using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_escuelas
{
    public partial class Listado : Form
    {
        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=tp_lab4;Trusted_Connection=True;";

        public Listado()
        {
            InitializeComponent();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            LoadData("Alumnos", dataGridView1);
            LoadData("Profesores", dataGridView2);
            LoadData("Materia", dataGridView3); 
        }


        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) 
            {
                LoadData("Alumnos", dataGridView1);
            }
            else if (tabControl1.SelectedIndex == 1) 
            {
                LoadData("Profesores", dataGridView2);
            }
            else if (tabControl1.SelectedIndex == 2) 
            {
                LoadData("Materia", dataGridView3);
            }
        }

        int CalcularEdad(DateTime fechaNacimiento)
        { 
            DateTime fechaActual = DateTime.Today;
            int edad = fechaActual.Year - fechaNacimiento.Year;
            return edad;
        }

        void LoadData(string tableName, DataGridView targetGridView)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "";

                    if (tableName == "Alumnos")
                    {
                        query = "SELECT id_alumno, nombre, apellido, dni, telefono, fecha_nac FROM Alumnos";
                    }
                    else if (tableName == "Profesores")
                    {
                        query = "SELECT id_profesor, nombre, dni, telefono, fecha_nac FROM Profesores";
                    }
                    else if (tableName == "Materia")
                    {
                        query = "SELECT id_materia, nombre  FROM Materia";
                    }
                    else
                    {
                        throw new ArgumentException("Tabla no válida.");
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        if (tableName == "Alumnos" || tableName == "Profesores")
                        {
                            if (!dataTable.Columns.Contains("Edad"))
                            {
                                dataTable.Columns.Add("Edad", typeof(int));
                            }
                            foreach (DataRow row in dataTable.Rows) { DateTime fechaNac = Convert.ToDateTime(row["fecha_nac"]); 
                                int edad = CalcularEdad(fechaNac);
                                row["Edad"] = edad; } }
                        targetGridView.DataSource = dataTable;

                        MessageBox.Show($"Cargadas {dataTable.Rows.Count} filas de {tableName}.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)           
        {
            if (tabControl1.SelectedIndex == 0)
            {
                LoadData("Alumnos", dataGridView1, "procedimiento");
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                LoadData("Profesores", dataGridView2, "procedimiento1");
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                LoadData("Materia", dataGridView2, "procedimiento3");
            }
        }

        private void LoadData(string tableName, DataGridView targetGridView, string procedureName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@cTexto", textBox1.Text);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    targetGridView.DataSource = dataTable;
                    MessageBox.Show($"Cargadas {dataTable.Rows.Count} filas de {tableName}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 inicio = new Form1();
            inicio.Show();
            this.Hide();
        }

        private void Inicio_Click(object sender, EventArgs e)
        {
            Form1 inicio = new Form1();
            inicio.Show();
            this.Hide();
        }
    }
    }