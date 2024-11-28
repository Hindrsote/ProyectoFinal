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
            // Asigna el evento para el cambio de pestaña
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            // Carga inicial de los datos con el DataGridView correspondiente
            LoadData("Alumnos", dataGridView1);
            LoadData("Profesores", dataGridView2);
            LoadData("Materias", dataGridView3); // Si tienes la pestaña Materias
        }
        

        // Evento que se ejecuta cuando se cambia de pestaña
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // Tab Alumnos
            {
                LoadData("Alumnos", dataGridView1);
            }
            else if (tabControl1.SelectedIndex == 1) // Tab Profesores
            {
                LoadData("Profesores", dataGridView2);
            }
            else if (tabControl1.SelectedIndex == 2) // Tab Materias (si agregas esta funcionalidad)
            {
                LoadData("Materias", dataGridView3);
            }
        }
        void LoadData(string tableName, DataGridView targetGridView)  // Método para cargar los datos en el DataGridView
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "";

                    // Usamos un bloque if-else para verificar la tabla y asignar la consulta adecuada
                    if (tableName == "Alumnos")
                    {
                        query = "SELECT id_alumno, nombre, apellido, dni, telefono, fecha_nac, id_materia FROM Alumnos";
                    }
                    else if (tableName == "Profesores")
                    {
                        query = "SELECT id_profesor, nombre, dni, telefono, fecha_nac, id_materia FROM Profesores";
                    }
                    else if (tableName == "Materias")
                    {
                        query = "SELECT id_materia, nombre, descripcion FROM Materias"; // Si hay una tabla de materias
                    }
                    else
                    {
                        // En caso de que no se pase una tabla válida
                        throw new ArgumentException("Tabla no válida.");
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Asignar los datos al DataGridView correspondiente
                        targetGridView.DataSource = dataTable;

                        // Mostrar la cantidad de filas obtenidas (temporalmente)
                        MessageBox.Show($"Cargadas {dataTable.Rows.Count} filas de {tableName}.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) //LISTAR 
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
                LoadData("Materias", dataGridView2, "procedimiento3");
            }
        }

        // Método para cargar los datos dependiendo del procedimiento y DataGridView correspondiente
        private void LoadData(string tableName, DataGridView targetGridView, string procedureName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Preparar la consulta para llamar al procedimiento almacenado
                    SqlCommand command = new SqlCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Si el procedimiento tiene parámetros, puedes añadirlos aquí.
                    command.Parameters.AddWithValue("@cTexto", textBox1.Text);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Asigna los datos al DataGridView correspondiente
                    targetGridView.DataSource = dataTable;

                    // Mostrar la cantidad de filas obtenidas (temporalmente)
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
    }
}
