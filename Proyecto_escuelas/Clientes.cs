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
    public partial class Clientes : Form
    {
        public Clientes()
        {
            InitializeComponent();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            LoadData("Articulos", dataGridView1, 1);
            LoadData("Articulos", dataGridView2, 0);
        }
        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;";
        
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // Pestaña 1: Visible = 1
            {
                LoadData("Articulos", dataGridView1, 1);
                button4.Visible = false;
            }
            else if (tabControl1.SelectedIndex == 1) // Pestaña 2: Visible = 0
            {
                LoadData("Articulos", dataGridView2, 0);
                button4.Visible = true;
            }
        }
        void LoadData(string tableName, DataGridView targetGridView, int visibleFlag)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "";

                    if (tableName == "Articulos")
                    {
                        // Filtrar por el valor de la columna Visible
                        query = @"SELECT ArticuloID, Nombre, Precio, Descripcion, Stock, FechaAlta, Estado 
                          FROM Articulos 
                          WHERE Visible = @VisibleFlag";
                    }
                    else
                    {
                        MessageBox.Show("Tabla no reconocida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetro para el filtro de Visible
                        command.Parameters.AddWithValue("@VisibleFlag", visibleFlag);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            targetGridView.DataSource = dataTable;
                        }
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

        private void button1_Click_2(object sender, EventArgs e)
        {
            // Cadena de conexión
            string connectionString = "Server=DESKTOP-2MVFTUI;Database=Tienda;Trusted_Connection=True;";

            // Validar si estamos en la pestaña correcta
            if (tabControl1.SelectedTab != tabPage1)
            {
                MessageBox.Show("Por ahora solo puedes agregar artículos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar los campos obligatorios
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Por favor, completa los campos obligatorios: Nombre, Precio y Stock.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Conversión y validación de datos numéricos
            if (!decimal.TryParse(textBox2.Text, out decimal precio))
            {
                MessageBox.Show("El campo 'Precio' debe ser un valor numérico.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Consulta SQL para insertar en la tabla 'Articulos'
            string query = @"INSERT INTO Articulos (Nombre, Descripcion, Precio, Stock) 
                     VALUES (@Nombre, @Descripcion, @Precio";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetros SQL
                        command.Parameters.AddWithValue("@Nombre", textBox1.Text);
                        command.Parameters.AddWithValue("@Descripcion", textBox3.Text);
                        command.Parameters.AddWithValue("@Precio", precio);

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Artículo agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData("Articulos", dataGridView1, 1); // Refrescar DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No se pudo agregar el artículo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el artículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) //MODIFICAR
        {
            string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;";

            // Validar si estamos en la pestaña correcta
            if (tabControl1.SelectedTab != tabPage1)
            {
                MessageBox.Show("Por ahora solo puedes modificar artículos en esta pestaña.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar los campos obligatorios
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Por favor, completa los campos obligatorios: Nombre, Precio y Stock.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Conversión y validación de datos numéricos
            if (!decimal.TryParse(textBox2.Text, out decimal precio))
            {
                MessageBox.Show("El campo 'Precio' debe ser un valor numérico.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBox4.Text, out int stock))
            {
                MessageBox.Show("El campo 'Stock' debe ser un valor numérico entero.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Obtener el ID del artículo seleccionado
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
            {
                MessageBox.Show("Por favor selecciona un artículo para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int articuloID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ArticuloID"].Value); // Asume que la columna ID se llama 'ArticuloID'

            // Consulta SQL para actualizar el registro en la tabla 'Articulos'
            string query = @"UPDATE Articulos 
                     SET Nombre = @Nombre, 
                         Descripcion = @Descripcion, 
                         Precio = @Precio, 
                         Stock = @Stock 
                     WHERE ArticuloID = @ArticuloID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetros SQL
                        command.Parameters.AddWithValue("@Nombre", textBox1.Text);
                        command.Parameters.AddWithValue("@Descripcion", textBox3.Text);
                        command.Parameters.AddWithValue("@Precio", precio);
                        command.Parameters.AddWithValue("@Stock", stock);
                        command.Parameters.AddWithValue("@ArticuloID", articuloID);

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Artículo modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData("Articulos", dataGridView1, 1); // Refrescar el DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No se pudo modificar el artículo. Verifica el ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el artículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) //ELIMINAR !!!!!!!
        {
            string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar si estamos en la pestaña correcta
                    if (tabControl1.SelectedTab != tabPage1)
                    {
                        MessageBox.Show("Solo puedes modificar registros de la tabla 'Articulos'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validar que haya una fila seleccionada en el DataGridView
                    if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
                    {
                        MessageBox.Show("Por favor selecciona un artículo para modificar su visibilidad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Obtener el ID del artículo seleccionado (columna 'ArticuloID')
                    int articuloID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ArticuloID"].Value);

                    // Confirmar la acción
                    var confirmResult = MessageBox.Show(
                        "¿Estás seguro que deseas marcar este artículo como 'no visible'?",
                        "Confirmar acción",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirmResult != DialogResult.Yes)
                        return;

                    // Consulta SQL para actualizar la columna "Visible" a 0
                    string query = "UPDATE Articulos SET Visible = 0 WHERE ArticuloID = @ArticuloID";

                    // Ejecutar la consulta
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArticuloID", articuloID);
                        int rowsAffected = command.ExecuteNonQuery();

                        MessageBox.Show(rowsAffected > 0
                            ? "El artículo se ha marcado como 'no visible'."
                            : "No se pudo modificar la visibilidad del artículo. Verifica el ID.",
                            "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Refrescar el DataGridView después de la actualización
                LoadData("Articulos", dataGridView1, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la visibilidad del artículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;";
            try
            {
                if (dataGridView2.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, selecciona un artículo para restaurar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el ID del artículo seleccionado
                int articuloID = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ArticuloID"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Actualizar la columna Visible a 1
                    string query = @"UPDATE Articulos 
                             SET Visible = 1 
                             WHERE ArticuloID = @ArticuloID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArticuloID", articuloID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Artículo restaurado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Recargar los datos en DataGridView2
                            LoadData("Articulos", dataGridView2, 0);

                            // Opcional: Recargar también los datos visibles en DataGridView1
                            LoadData("Articulos", dataGridView1, 1);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo restaurar el artículo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el artículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
