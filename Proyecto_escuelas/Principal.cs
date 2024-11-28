using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proyecto_escuelas
{
    public partial class Principal : Form
    {
        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=tp_lab4;Trusted_Connection=True;";
        public Principal()
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 inicio = new Form1();
            inicio.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e) //AÑADIR o AGREGAR!!!!!!!!!!!!
        {
            // Validaciones de entrada
            if (!Regex.IsMatch(textBox1.Text, @"^[a-zA-Z]+$")) // Solo letras en "nombre"
            {
                MessageBox.Show("El campo 'Nombre' solo debe contener letras.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(textBox2.Text, @"^[a-zA-Z]+$")) // Solo letras en "apellido"
            {
                MessageBox.Show("El campo 'Apellido' solo debe contener letras.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(textBox3.Text, @"^\d+$")) // Solo números en "dni"
            {
                MessageBox.Show("El campo 'DNI' solo debe contener números.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(textBox4.Text, @"^\d+$")) // Solo números en "telefono"
            {
                MessageBox.Show("El campo 'Teléfono' solo debe contener números.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Extraer la fecha seleccionada
            DateTime fechaNac = dateTimePicker1.Value.Date;

            // Validar que la fecha de nacimiento sea al menos de 5 años atrás
            DateTime fechaLimite = DateTime.Today.AddYears(-5);
            if (fechaNac > fechaLimite)
            {
                MessageBox.Show("La fecha de nacimiento debe ser al menos de hace 5 años.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Define la cadena de conexión
            string connectionString = @"Server=localhost;Database=tp_lab4;Trusted_Connection=True;";

            // Variables para almacenar la tabla y la consulta SQL
            string tableName = "";
            string query = "";

            // Verifica qué pestaña está seleccionada
            if (tabControl1.SelectedTab == tabPage1)
            {
                tableName = "Alumnos"; // Tabla para Alumnos
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                tableName = "Profesores"; // Tabla para Profesores
            }
            else
            {
                MessageBox.Show("No se reconoce la pestaña seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Construir la consulta SQL
            query = $"INSERT INTO {tableName} (nombre, apellido, dni, telefono, fecha_nac) VALUES (@Nombre, @Apellido, @DNI, @Telefono, @FechaNac)";

            try
            {
                // Crear conexión
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Crear comando
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar parámetros
                        command.Parameters.AddWithValue("@Nombre", textBox1.Text);
                        command.Parameters.AddWithValue("@Apellido", textBox2.Text);
                        command.Parameters.AddWithValue("@DNI", textBox3.Text);
                        command.Parameters.AddWithValue("@Telefono", textBox4.Text);
                        command.Parameters.AddWithValue("@FechaNac", fechaNac);

                        // Abrir conexión
                        connection.Open();

                        // Ejecutar comando
                        int rowsAffected = command.ExecuteNonQuery();

                        // Mostrar mensaje de éxito
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"El registro ha sido añadido correctamente a la tabla {tableName}.",
                                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"No se pudo agregar el registro a la tabla {tableName}.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // Recargar los datos del DataGridView
                    if (tabControl1.SelectedIndex == 0) // Tab Alumnos
                    {
                        LoadData("Alumnos", dataGridView1);
                    }
                    else if (tabControl1.SelectedIndex == 1) // Tab Profesores
                    {
                        LoadData("Profesores", dataGridView2);
                    }
                    else if (tabControl1.SelectedIndex == 2) // Tab Materias (si aplicable)
                    {
                        LoadData("Materias", dataGridView3);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el registro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) //MODIFICAR
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Identificar la pestaña activa
                    string tableName, idColumn;
                    if (tabControl1.SelectedIndex == 0) // Alumnos
                    {
                        tableName = "Alumnos";
                        idColumn = "id_alumno";
                    }
                    else if (tabControl1.SelectedIndex == 1) // Profesores
                    {
                        tableName = "Profesores";
                        idColumn = "id_profesor";
                    }
                    else
                    {
                        MessageBox.Show("Modificación no válida en esta pestaña.");
                        return;
                    }

                    // Verificar si hay una fila seleccionada
                    int selectedRowIndex = tabControl1.SelectedIndex == 0
                        ? dataGridView1.CurrentCell.RowIndex
                        : dataGridView2.CurrentCell.RowIndex;

                    if (selectedRowIndex < 0)
                    {
                        MessageBox.Show("Por favor selecciona una fila para modificar.");
                        return;
                    }

                    // Obtener el ID de la fila seleccionada
                    var dataGridView = tabControl1.SelectedIndex == 0 ? dataGridView1 : dataGridView2;
                    int id = Convert.ToInt32(dataGridView.Rows[selectedRowIndex].Cells[0].Value);

                    // Comando de actualización
                    string query = $@"
                UPDATE {tableName}
                SET nombre = @nombre,
                    apellido = @apellido,
                    dni = @dni,
                    telefono = @telefono,
                    fecha_nac = @fecha_nac
                WHERE {idColumn} = @id";

                    // Extraer la fecha seleccionada
                    DateTime fechaNac = dateTimePicker1.Value.Date;

                    // Validar que la fecha de nacimiento sea al menos de 5 años atrás
                    DateTime fechaLimite = DateTime.Today.AddYears(-5);
                    if (fechaNac > fechaLimite)
                    {
                        MessageBox.Show("La fecha de nacimiento debe ser al menos de hace 5 años.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", textBox1.Text);
                        command.Parameters.AddWithValue("@apellido", textBox2.Text);
                        command.Parameters.AddWithValue("@dni", textBox3.Text);
                        command.Parameters.AddWithValue("@telefono", textBox4.Text);
                        command.Parameters.AddWithValue("@fecha_nac", fechaNac);
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show(rowsAffected > 0
                            ? "Registro modificado exitosamente."
                            : "No se pudo modificar el registro.");
                    }
                }

                if (tabControl1.SelectedIndex == 0) // Tab Alumnos
                {
                    LoadData("Alumnos", dataGridView1);
                }
                else if (tabControl1.SelectedIndex == 1) // Tab Profesores
                {
                    LoadData("Profesores", dataGridView2);
                }
                else if (tabControl1.SelectedIndex == 2) // Tab Materias (si aplicable)
                {
                    LoadData("Materias", dataGridView3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) //ELIMINAR !!!!!!!
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Identificar la pestaña activa
                    string tableName, idColumn;
                    if (tabControl1.SelectedIndex == 0) // Alumnos
                    {
                        tableName = "Alumnos";
                        idColumn = "id_alumno";
                    }
                    else if (tabControl1.SelectedIndex == 1) // Profesores
                    {
                        tableName = "Profesores";
                        idColumn = "id_profesor";
                    }
                    else
                    {
                        MessageBox.Show("Eliminación no válida en esta pestaña.");
                        return;
                    }

                    // Verificar si hay una fila seleccionada
                    int selectedRowIndex = tabControl1.SelectedIndex == 0
                        ? dataGridView1.CurrentCell.RowIndex
                        : dataGridView2.CurrentCell.RowIndex;

                    if (selectedRowIndex < 0)
                    {
                        MessageBox.Show("Por favor selecciona una fila para eliminar.");
                        return;
                    }

                    // Obtener el ID de la fila seleccionada
                    var dataGridView = tabControl1.SelectedIndex == 0 ? dataGridView1 : dataGridView2;
                    int id = Convert.ToInt32(dataGridView.Rows[selectedRowIndex].Cells[0].Value);

                    // Confirmar eliminación
                    var confirmResult = MessageBox.Show(
                        "¿Estás seguro que deseas eliminar este registro?",
                        "Confirmar eliminación",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirmResult != DialogResult.Yes)
                        return;

                    // Comando de eliminación
                    string query = $@"DELETE FROM {tableName} WHERE {idColumn} = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        MessageBox.Show(rowsAffected > 0
                            ? "Registro eliminado exitosamente."
                            : "No se pudo eliminar el registro.");
                    }
                }

                if (tabControl1.SelectedIndex == 0) // Tab Alumnos
                {
                    LoadData("Alumnos", dataGridView1);
                }
                else if (tabControl1.SelectedIndex == 1) // Tab Profesores
                {
                    LoadData("Profesores", dataGridView2);
                }
                else if (tabControl1.SelectedIndex == 2) // Tab Materias (si aplicable)
                {
                    LoadData("Materias", dataGridView3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
