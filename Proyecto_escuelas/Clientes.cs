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
            LoadData("Clientes", dataGridView1, 1);
            LoadData("Clientes", dataGridView2, 0);
        }
        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;";

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // Pestaña 1: Visible = 1
            {
                LoadData("Clientes", dataGridView1, 1);  // Filtrar por clientes visibles
            }
            else if (tabControl1.SelectedIndex == 1) // Pestaña 2: Moroso = 1
            {
                LoadData("Clientes", dataGridView2, 2);  // Filtrar por clientes morosos
            }
        }

        void LoadData(string tableName, DataGridView targetGridView, int filterFlag)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "";

                    if (tableName == "Clientes")
                    {
                        // Filtrar por la columna Visible o Moroso según el filtro
                        if (filterFlag == 1)
                        {
                            // Filtrar clientes visibles
                            query = @"SELECT ClienteID, Nombre, Apellido, Telefono, Direccion, FechaNacimiento, Anotaciones , Moroso
                              FROM Clientes WHERE Moroso = 0 AND Visible = 1";
                        }
                        else if (filterFlag == 2)
                        {
                            // Filtrar clientes morosos
                            query = @"SELECT ClienteID, Nombre, Apellido, Telefono, Direccion, FechaNacimiento, Anotaciones , Moroso
                              FROM Clientes WHERE Moroso = 1";
                        }
                        else
                        {
                            // Caso por defecto, si el filtro no es 1 ni 2
                            query = @"SELECT ClienteID, Nombre, Apellido, Telefono, Direccion, FechaNacimiento, Anotaciones , Moroso
                              FROM Clientes";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tabla no reconocida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
            Form1 inicio = new Form1(UsuarioActual.Nombre, UsuarioActual.Rol); // Cambia "UsuarioActual" y "Vendedor" según corresponda
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
            
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) //MODIFICAR
        {
            
        }

        private void button3_Click(object sender, EventArgs e) // ELIMINAR O CAMBIAR ESTADO DE CLIENTE
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void Inicio_Click(object sender, EventArgs e)
        {
            Form1 inicio = new Form1(UsuarioActual.Nombre, UsuarioActual.Rol); // Cambia "UsuarioActual" y "Vendedor" según corresponda
            inicio.Show();
            this.Hide();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e) // activar
        {
            string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;";

            try
            {
                // Validar que se haya seleccionado un cliente en el DataGridView
                if (dataGridView2.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, selecciona un cliente para restaurar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el ID del cliente seleccionado
                int clienteID = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ClienteID"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Actualizar la columna Visible a 1 para restaurar el cliente
                    string query = @"UPDATE Clientes 
                             SET Visible = 1 
                             WHERE ClienteID = @ClienteID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteID", clienteID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente restaurado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Recargar los datos en DataGridView2 (clientes no visibles)
                            LoadData("Clientes", dataGridView2, 0);

                            // Opcional: Recargar también los datos visibles en DataGridView1 (clientes visibles)
                            LoadData("Clientes", dataGridView1, 1);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo restaurar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
       string.IsNullOrWhiteSpace(textBox2.Text) ||
       string.IsNullOrWhiteSpace(textBox3.Text) ||
       string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Por favor, completa los campos obligatorios: Nombre, Apellido, Teléfono y Dirección.",
                                "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Obtener los valores de los campos
            string nombre = textBox1.Text;
            string apellido = textBox2.Text;
            string telefono = textBox3.Text;
            string direccion = textBox4.Text;
            DateTime fechaNacimiento = dateTimePicker1.Value; // Obtener la fecha de nacimiento

            // Consulta SQL para insertar en la tabla 'Clientes'
            string query = @"INSERT INTO Clientes (Nombre, Apellido, Telefono, Direccion, FechaNacimiento, Anotaciones, Visible, Moroso ) 
                     VALUES (@Nombre, @Apellido, @Telefono, @Direccion, @FechaNacimiento, @Anotaciones, @Visible, @Moroso)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetros SQL
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Apellido", apellido);
                        command.Parameters.AddWithValue("@Telefono", telefono);
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);
                        command.Parameters.AddWithValue("@Anotaciones", textBox5.Text); // Para anotaciones adicionales
                        command.Parameters.AddWithValue("@Visible", 1);
                        command.Parameters.AddWithValue("@Moroso", 0);

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData("Clientes", dataGridView1, 1); // Refrescar DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No se pudo agregar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
       string.IsNullOrWhiteSpace(textBox2.Text) ||
       string.IsNullOrWhiteSpace(textBox3.Text) ||
       string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Por favor, completa los campos obligatorios: Nombre, Apellido, Teléfono y Dirección.",
                                "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que se haya seleccionado un cliente
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
            {
                MessageBox.Show("Por favor selecciona un cliente para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int clienteID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ClienteID"].Value); // Asume que la columna ID se llama 'ClienteID'

            // Obtener los nuevos valores
            string nombre = textBox1.Text;
            string apellido = textBox2.Text;
            string telefono = textBox3.Text;
            string direccion = textBox4.Text;
            DateTime fechaNacimiento = dateTimePicker1.Value;

            // Consulta SQL para actualizar el registro en la tabla 'Clientes'
            string query = @"UPDATE Clientes 
                     SET Nombre = @Nombre, 
                         Apellido = @Apellido, 
                         Telefono = @Telefono, 
                         Direccion = @Direccion, 
                         FechaNacimiento = @FechaNacimiento, 
                         Anotaciones = @Anotaciones 
                     WHERE ClienteID = @ClienteID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetros SQL
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Apellido", apellido);
                        command.Parameters.AddWithValue("@Telefono", telefono);
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);
                        command.Parameters.AddWithValue("@Anotaciones", textBox5.Text);
                        command.Parameters.AddWithValue("@ClienteID", clienteID);

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData("Clientes", dataGridView1, 1); // Refrescar el DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No se pudo modificar el cliente. Verifica el ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int clienteID;
                    int currentMoroso;

                    // Verificar que haya una fila seleccionada en el DataGridView adecuado
                    if (tabControl1.SelectedTab == tabPage1) // Si estamos en la pestaña 1
                    {
                        // Validar que haya una fila seleccionada en dataGridView1
                        if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
                        {
                            MessageBox.Show("Por favor selecciona un cliente para cambiar su estado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        clienteID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ClienteID"].Value);
                        currentMoroso = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Moroso"].Value);
                    }
                    else if (tabControl1.SelectedTab == tabPage2) // Si estamos en la pestaña 2
                    {
                        // Validar que haya una fila seleccionada en dataGridView2
                        if (dataGridView2.CurrentRow == null || dataGridView2.CurrentRow.Index < 0)
                        {
                            MessageBox.Show("Por favor selecciona un cliente para cambiar su estado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        clienteID = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ClienteID"].Value);
                        currentMoroso = Convert.ToInt32(dataGridView2.CurrentRow.Cells["Moroso"].Value);
                    }
                    else
                    {
                        // Si por alguna razón no estamos en ninguna de las dos pestañas, mostramos un mensaje
                        MessageBox.Show("Selecciona una pestaña válida para cambiar el estado del cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Si estamos en la pestaña 1 (Marcar como Moroso)
                    if (tabControl1.SelectedTab == tabPage1)
                    {
                        if (currentMoroso == 1)
                        {
                            MessageBox.Show("Este cliente ya está marcado como Moroso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;  // No hacer nada si ya es Moroso
                        }

                        // Confirmar la acción de marcarlo como moroso
                        var confirmResult = MessageBox.Show(
                            "¿Estás seguro que deseas marcar a este cliente como 'Moroso'?",
                            "Confirmar acción",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (confirmResult != DialogResult.Yes)
                            return;

                        // Consulta SQL para actualizar el valor de 'Moroso' a 1 (Moroso)
                        string query = "UPDATE Clientes SET Moroso = 1 WHERE ClienteID = @ClienteID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ClienteID", clienteID);
                            int rowsAffected = command.ExecuteNonQuery();

                            MessageBox.Show(rowsAffected > 0
                                ? "El cliente ha sido marcado como 'Moroso'."
                                : "No se pudo actualizar el estado del cliente. Verifica el ID.",
                                "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    // Si estamos en la pestaña 2 (Marcar como No Moroso)
                    else if (tabControl1.SelectedTab == tabPage2)
                    {
                        if (currentMoroso == 0)
                        {
                            MessageBox.Show("Este cliente ya está marcado como 'No Moroso'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;  // No hacer nada si ya es No Moroso
                        }

                        // Confirmar la acción de marcarlo como No Moroso
                        var confirmResult = MessageBox.Show(
                            "¿Estás seguro que deseas marcar a este cliente como 'No Moroso'?",
                            "Confirmar acción",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (confirmResult != DialogResult.Yes)
                            return;

                        // Consulta SQL para actualizar el valor de 'Moroso' a 0 (No Moroso)
                        string query = "UPDATE Clientes SET Moroso = 0 WHERE ClienteID = @ClienteID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ClienteID", clienteID);
                            int rowsAffected = command.ExecuteNonQuery();

                            MessageBox.Show(rowsAffected > 0
                                ? "El cliente ha sido marcado como 'No Moroso'."
                                : "No se pudo actualizar el estado del cliente. Verifica el ID.",
                                "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                // Refrescar el DataGridView después de la actualización
                LoadData("Clientes", dataGridView1, 1); // Actualiza la lista de clientes visibles
                LoadData("Clientes", dataGridView2, 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el estado del cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
