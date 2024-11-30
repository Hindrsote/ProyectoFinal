using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto_escuelas
{
    public partial class Inscripciones : Form

    {
        public class ListBoxItem
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=tp_lab4;Trusted_Connection=True;";
        public Inscripciones()
        {
            InitializeComponent();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            LoadData("Alumnos", dataGridView1);
            LoadData("Profesores", dataGridView2);
            LoadData("Materia", dataGridView6);
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)           // para VISUALIZAR
        {
            if (tabControl1.SelectedIndex == 0)
            {
                LoadData("Alumnos", dataGridView1);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                LoadData("Profesores", dataGridView2);
            }
            else if (tabControl2.SelectedIndex == 0)
            {
                LoadData("Materia", dataGridView6);
            }
        }
        void LoadData(string tableName, object targetControl)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "";

                    if (tableName == "Alumnos")
                    {
                        query = "SELECT id_alumno, nombre, apellido, dni, telefono, fecha_nac, materias FROM Alumnos";
                    }
                    else if (tableName == "Profesores")
                    {
                        query = "SELECT id_profesor, nombre, dni, telefono, fecha_nac, materias FROM Profesores";
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

                        if (targetControl is DataGridView gridView)
                        {
                            gridView.DataSource = dataTable;
                        }
                        else
                        {
                            throw new ArgumentException("El control de destino no es compatible.");
                        }

                        MessageBox.Show($"Cargadas {dataTable.Rows.Count} filas de {tableName}.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Inscripciones1(object sender, EventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

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
                        query = "SELECT id_alumno, nombre, apellido, dni, telefono, fecha_nac, materias FROM Alumnos";
                    }
                    else if (tableName == "Profesores")
                    {
                        query = "SELECT id_profesor, nombre, dni, telefono, fecha_nac, materias FROM Profesores";
                    }
                    else if (tableName == "Materia")
                    {
                        query = "SELECT id_materia, nombre, descripcion FROM Materia";
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


        private void button1_Click(object sender, EventArgs e) // AGREGAR MATERIAAAAAAAAAAAAAAAAAA
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
            string connectionString = "Server=DESKTOP-2MVFTUI;Database=tp_lab4;Trusted_Connection=True;";
            string query = "INSERT INTO Materia (nombre) VALUES (@Nombre)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", textBox1.Text.Trim());
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("La materia ha sido añadida correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox1.Clear();
                            LoadData("Materia", dataGridView6);
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

        private void button2_Click(object sender, EventArgs e) ////////////////// Agregar materias a profe o estudiante
        {
            if ((dataGridView1.SelectedRows.Count > 0 || dataGridView2.SelectedRows.Count > 0) && dataGridView6.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridView activeGridView = dataGridView1.SelectedRows.Count > 0 ? dataGridView1 : dataGridView2;
                    int idSeleccionado = Convert.ToInt32(activeGridView.SelectedRows[0].Cells[activeGridView == dataGridView1 ? "id_alumno" : "id_profesor"].Value);
                    string tipoSeleccionado = activeGridView == dataGridView1 ? "Alumnos" : "Profesores";
                    string nombreMateria = dataGridView6.SelectedRows[0].Cells["nombre"].Value.ToString().Trim();
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string selectQuery = tipoSeleccionado == "Alumnos"
                            ? "SELECT materias FROM Alumnos WHERE id_alumno = @id"
                            : "SELECT materias FROM Profesores WHERE id_profesor = @id";

                        string materiasActuales = "";

                        using (SqlCommand selectCmd = new SqlCommand(selectQuery, connection))
                        {
                            selectCmd.Parameters.AddWithValue("@id", idSeleccionado);
                            using (SqlDataReader reader = selectCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    materiasActuales = reader["materias"] != DBNull.Value ? reader["materias"].ToString() : "";
                                }
                            }
                        }

                        if (!materiasActuales.Split(',').Select(m => m.Trim()).Contains(nombreMateria))
                        {

                            materiasActuales = string.IsNullOrEmpty(materiasActuales)
                                ? nombreMateria
                                : $"{materiasActuales}, {nombreMateria}";

                            string updateQuery = tipoSeleccionado == "Alumnos"
                                ? "UPDATE Alumnos SET materias = @materias WHERE id_alumno = @id"
                                : "UPDATE Profesores SET materias = @materias WHERE id_profesor = @id";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@materias", materiasActuales);
                                updateCmd.Parameters.AddWithValue("@id", idSeleccionado);

                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Dime que sí, dime que sí");
                                    LoadData("Alumnos", dataGridView1);
                                    LoadData("Profesores", dataGridView2);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo realizar la inscripción.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("La materia ya está inscrita.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al inscribir: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un alumno/profesor y una materia.");
            }
        }

        private void button3_Click(object sender, EventArgs e) // Eliminar materias de dataGridView6
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int selectedRowIndex = dataGridView6.CurrentCell?.RowIndex ?? -1;

                    if (selectedRowIndex < 0)
                    {
                        MessageBox.Show("Por favor selecciona una fila para eliminar.");
                        return;
                    }

                    int idMateria = Convert.ToInt32(dataGridView6.Rows[selectedRowIndex].Cells["id_materia"].Value);
                    string checkQuery = @"
                SELECT COUNT(*) AS Inscripciones
                FROM (
                    SELECT materias FROM Alumnos WHERE materias LIKE '%' + @nombreMateria + '%'
                    UNION ALL
                    SELECT materias FROM Profesores WHERE materias LIKE '%' + @nombreMateria + '%'
                ) AS Inscritos";

                    string nombreMateria = dataGridView6.Rows[selectedRowIndex].Cells["nombre"].Value.ToString().Trim();

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@nombreMateria", nombreMateria);
                        int inscripciones = (int)checkCmd.ExecuteScalar();

                        if (inscripciones > 0)
                        {
                            MessageBox.Show($"No se puede eliminar. Hay {inscripciones} inscritos en esta materia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    var confirmResult = MessageBox.Show(
                        "¿Estás seguro de eliminar esta materia?",
                        "Confirmación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirmResult != DialogResult.Yes)
                        return;
                    string deleteQuery = "DELETE FROM Materia WHERE id_materia = @id_materia";

                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@id_materia", idMateria);
                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        MessageBox.Show(rowsAffected > 0
                            ? "Materia eliminada exitosamente."
                            : "Error al eliminar la materia.");
                    }
                }

                // Yo aqui te espero solo vente vente vente
                LoadData("Materia", dataGridView6);
                LoadData("Alumnos", dataGridView1);
                LoadData("Profesores", dataGridView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ((dataGridView1.SelectedRows.Count > 0 || dataGridView2.SelectedRows.Count > 0) && dataGridView6.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridView activeGridView = dataGridView1.SelectedRows.Count > 0 ? dataGridView1 : dataGridView2;
                    int idSeleccionado = Convert.ToInt32(activeGridView.SelectedRows[0].Cells[activeGridView == dataGridView1 ? "id_alumno" : "id_profesor"].Value);
                    string tipoSeleccionado = activeGridView == dataGridView1 ? "Alumnos" : "Profesores";
                    string nombreMateria = dataGridView6.SelectedRows[0].Cells["nombre"].Value.ToString().Trim();
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string selectQuery = tipoSeleccionado == "Alumnos"
                            ? "SELECT materias FROM Alumnos WHERE id_alumno = @id"
                            : "SELECT materias FROM Profesores WHERE id_profesor = @id";
                        string materiasActuales = "";
                        using (SqlCommand selectCmd = new SqlCommand(selectQuery, connection))
                        {
                            selectCmd.Parameters.AddWithValue("@id", idSeleccionado);
                            using (SqlDataReader reader = selectCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    materiasActuales = reader["materias"] != DBNull.Value ? reader["materias"].ToString() : "";
                                }
                            }
                        }
                        var materiasList = materiasActuales.Split(',').Select(m => m.Trim()).ToList();
                        if (materiasList.Contains(nombreMateria))
                        {
                            materiasList.Remove(nombreMateria);
                            string nuevasMaterias = string.Join(", ", materiasList);

                            string updateQuery = tipoSeleccionado == "Alumnos"
                                ? "UPDATE Alumnos SET materias = @materias WHERE id_alumno = @id"
                                : "UPDATE Profesores SET materias = @materias WHERE id_profesor = @id";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@materias", nuevasMaterias);
                                updateCmd.Parameters.AddWithValue("@id", idSeleccionado);

                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Inscripción deshecha con éxito.");
                                    LoadData("Alumnos", dataGridView1);
                                    LoadData("Profesores", dataGridView2);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo deshacer la inscripción.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("La materia no está inscrita.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al deshacer la inscripción: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un alumno/profesor y una materia.");
            }
        }

        private void Inicio_Click(object sender, EventArgs e)
        {
            Form1 inicio = new Form1();
            inicio.Show();
            this.Hide();
        }
    }
}

