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
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            LoadData("Alumnos", dataGridView1);
            LoadData("Profesores", dataGridView2);
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
                        query = "SELECT id_alumno, nombre, apellido, dni, telefono, fecha_nac, id_materia FROM Alumnos";
                    }
                    else if (tableName == "Profesores")
                    {
                        query = "SELECT id_profesor, nombre, dni, telefono, fecha_nac, id_materia FROM Profesores";
                    }
                    else if (tableName == "Materia")
                    {
                        query = "SELECT id_materia, nombre FROM Materia"; 
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

        private void button1_Click_2(object sender, EventArgs e) //////////////////////////////////AÑADIR o AGREGAR!!!!!!!!!!!!
        {

            if (!Regex.IsMatch(textBox1.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("El campo 'Nombre' solo debe contener letras.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(textBox2.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("El campo 'Apellido' solo debe contener letras.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validación del DNI con un rango de 7 a 8 dígitos
            if (!Regex.IsMatch(textBox3.Text, @"^\d{7,8}$"))
            {
                MessageBox.Show("El campo 'DNI' debe contener entre 7 y 8 dígitos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validación del Teléfono (números entre 7 y 15 dígitos)
            if (!Regex.IsMatch(textBox4.Text, @"^\d{7,15}$"))
            {
                MessageBox.Show("El campo 'Teléfono' debe contener entre 7 y 15 dígitos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime fechaNac = dateTimePicker1.Value.Date;

            DateTime fechaLimite = DateTime.Today.AddYears(-5);
            if (fechaNac > fechaLimite)
            {
                MessageBox.Show("La fecha de nacimiento debe ser al menos de hace 5 años.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = @"Server=localhost;Database=tp_lab4;Trusted_Connection=True;";

            string tableName = "";
            string query = "";

            if (tabControl1.SelectedTab == tabPage1)
            {
                tableName = "Alumnos";
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                tableName = "Profesores";
            }
            else
            {
                MessageBox.Show("No se reconoce la pestaña seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            query = $"INSERT INTO {tableName} (nombre, apellido, dni, telefono, fecha_nac) VALUES (@Nombre, @Apellido, @DNI, @Telefono, @FechaNac)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string checkQuery = $"SELECT COUNT(*) FROM {tableName} WHERE dni = @DNI";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@DNI", textBox3.Text);
                        connection.Open();
                        int existingRecords = Convert.ToInt32(checkCommand.ExecuteScalar());
                        if (existingRecords > 0)
                        {
                            MessageBox.Show("DNI DUPLICADO", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Inserción del nuevo registro
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", textBox1.Text);
                        command.Parameters.AddWithValue("@Apellido", textBox2.Text);
                        command.Parameters.AddWithValue("@DNI", textBox3.Text);
                        command.Parameters.AddWithValue("@Telefono", textBox4.Text);
                        command.Parameters.AddWithValue("@FechaNac", fechaNac);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Exitooo {tableName}.",
                                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"No se pudo agregar el registro a la tabla {tableName}.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (tabControl1.SelectedIndex == 0)
                    {
                        LoadData("Alumnos", dataGridView1);
                    }
                    else if (tabControl1.SelectedIndex == 1)
                    {
                        LoadData("Profesores", dataGridView2);
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
                    string tableName, idColumn;
                    if (tabControl1.SelectedIndex == 0) 
                    {
                        tableName = "Alumnos";
                        idColumn = "id_alumno";
                    }
                    else if (tabControl1.SelectedIndex == 1) 
                    {
                        tableName = "Profesores";
                        idColumn = "id_profesor";
                    }
                    else
                    {
                        MessageBox.Show("Modificación no válida en esta pestaña.");
                        return;
                    }

                    int selectedRowIndex = tabControl1.SelectedIndex == 0
                        ? dataGridView1.CurrentCell.RowIndex
                        : dataGridView2.CurrentCell.RowIndex;

                    if (selectedRowIndex < 0)
                    {
                        MessageBox.Show("Por favor selecciona una fila para modificar.");
                        return;
                    }

                    var dataGridView = tabControl1.SelectedIndex == 0 ? dataGridView1 : dataGridView2;
                    int id = Convert.ToInt32(dataGridView.Rows[selectedRowIndex].Cells[0].Value);

                    string query = $@"
                UPDATE {tableName}
                SET nombre = @nombre,
                    apellido = @apellido,
                    dni = @dni,
                    telefono = @telefono,
                    fecha_nac = @fecha_nac
                WHERE {idColumn} = @id";

                    DateTime fechaNac = dateTimePicker1.Value.Date;

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
                            ? "Modificado"
                            : "No se pudo modificar el registro.");
                    }
                }

                if (tabControl1.SelectedIndex == 0) 
                {
                    LoadData("Alumnos", dataGridView1);
                }
                else if (tabControl1.SelectedIndex == 1) 
                {
                    LoadData("Profesores", dataGridView2);
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

                    string tableName, idColumn;
                    if (tabControl1.SelectedIndex == 0)
                    {
                        tableName = "Alumnos";
                        idColumn = "id_alumno";
                    }
                    else if (tabControl1.SelectedIndex == 1) 
                    {
                        tableName = "Profesores";
                        idColumn = "id_profesor";
                    }
                    else
                    {
                        MessageBox.Show("Eliminación no válida en esta pestaña.");
                        return;
                    }

                    int selectedRowIndex = tabControl1.SelectedIndex == 0
                        ? dataGridView1.CurrentCell.RowIndex
                        : dataGridView2.CurrentCell.RowIndex;

                    if (selectedRowIndex < 0)
                    {
                        MessageBox.Show("Por favor selecciona una fila para eliminar.");
                        return;
                    }

                    var dataGridView = tabControl1.SelectedIndex == 0 ? dataGridView1 : dataGridView2;
                    int id = Convert.ToInt32(dataGridView.Rows[selectedRowIndex].Cells[0].Value);
                    var confirmResult = MessageBox.Show(
                        "¿Estás seguro que deseas eliminar este registro?",
                        "Confirmar eliminación",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirmResult != DialogResult.Yes)
                        return;
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

                if (tabControl1.SelectedIndex == 0) 
                {
                    LoadData("Alumnos", dataGridView1);
                }
                else if (tabControl1.SelectedIndex == 1) 
                {
                    LoadData("Profesores", dataGridView2);
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
