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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proyecto_escuelas
{
    public partial class Inscripciones : Form
    {
        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=tp_lab4;Trusted_Connection=True;";
        public Inscripciones()
        {
            InitializeComponent();
            // Asigna el evento para el cambio de pestaña
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            // Carga inicial de los datos con el DataGridView correspondiente
            LoadData("Alumnos", dataGridView1);
            LoadData("Profesores", dataGridView2);
            LoadData("Materia", dataGridView3); // Si tienes la pestaña Materias
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e) // para VISUALIZAR
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
                LoadData("Materia", dataGridView3);
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
                    else if (tableName == "Materia")
                    {
                        query = "SELECT id_materia, nombre FROM Materia"; // Si hay una tabla de materias
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
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("El campo 'Nombre' no puede estar vacío.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox1.Text.Length > 100)
            {
                MessageBox.Show("El campo 'Nombre' no puede exceder los 100 caracteres.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Construir la consulta SQL
            string connectionString = "Server=DESKTOP-2MVFTUI;Database=tp_lab4;Trusted_Connection=True;"; // Reemplaza con tu cadena de conexión.
            string query = "INSERT INTO Materia (nombre) VALUES (@Nombre)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar parámetros
                        command.Parameters.AddWithValue("@Nombre", textBox1.Text.Trim());

                        // Abrir conexión
                        connection.Open();

                        // Ejecutar el comando
                        int rowsAffected = command.ExecuteNonQuery();

                        // Confirmar al usuario
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("La materia ha sido añadida correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Opcional: recargar datos en DataGridView o limpiar los campos
                            textBox1.Clear();
                            LoadData("Materia", dataGridView1); // Asegúrate de tener un método LoadData que recargue los datos en el DataGridView.
                        }
                        else
                        {
                            MessageBox.Show("No se pudo agregar la materia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
