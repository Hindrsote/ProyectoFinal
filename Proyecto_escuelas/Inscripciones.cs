using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using static Proyecto_escuelas.Form1;

namespace Proyecto_escuelas
{
    public partial class Inscripciones : Form

    {
        private List<ArticuloSeleccionado> articulosSeleccionados = new List<ArticuloSeleccionado>();

        // Método para agregar un artículo a la lista y actualizar el DataGridView
        public void AgregarArticuloSeleccionado(ArticuloSeleccionado articulo)
        {
            articulosSeleccionados.Add(articulo);
            ActualizarDataGridView();
        }

        // Método para actualizar el DataGridView con los artículos seleccionados
        private void ActualizarDataGridView()
        {
            dataGridView1.DataSource = null; // Limpiar el origen de datos
            dataGridView1.DataSource = articulosSeleccionados;

            // Configurar columnas si es necesario
            dataGridView1.Columns["Nombre"].HeaderText = "Nombre del Artículo";
            dataGridView1.Columns["Precio"].HeaderText = "Precio";
            dataGridView1.Columns["Cantidad"].HeaderText = "Cantidad";
            dataGridView1.Columns["Stock"].HeaderText = "Stock";
            // Asegúrate de que el ArticuloID esté oculto si no deseas mostrarlo al usuario
            if (dataGridView1.Columns.Contains("ArticuloID"))
            {
                dataGridView1.Columns["ArticuloID"].Visible = false;
            }
        }
        public Inscripciones()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (Ventacarga ventacargaForm = new Ventacarga(articulosSeleccionados))
            {

                ventacargaForm.Owner = this; // Establece el formulario actual como dueño
                ventacargaForm.ShowDialog();
                ActualizarDataGridView();
            }
        }

        public class ListBoxItem
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDb;Trusted_Connection=True;";
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)           // para VISUALIZAR
        {
            if (tabControl1.SelectedIndex == 0)
            {
            }
            else if (tabControl2.SelectedIndex == 0)
            {
            }
        }



        private void Inscripciones1(object sender, EventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay artículos seleccionados para la venta.");
                return;
            }

            decimal montoVentaTotal = 0;
            List<string> articulosVendidos = new List<string>(); // Lista para almacenar los nombres de los artículos vendidos

            // Obtener el ClienteID del DataGridView6 (cliente seleccionado)
            if (dataGridView6.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un cliente.");
                return;
            }

            int clienteID = Convert.ToInt32(dataGridView6.SelectedRows[0].Cells["ClienteID"].Value);
            string clienteNombre = dataGridView6.SelectedRows[0].Cells["Nombre"].Value.ToString();
            string clienteApellido = dataGridView6.SelectedRows[0].Cells["Apellido"].Value.ToString();

            // Obtener el medio de pago seleccionado del ComboBox
            string medioPago = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(medioPago))
            {
                MessageBox.Show("Por favor, seleccione un medio de pago.");
                return;
            }

            // Recorrer todas las filas de DataGridView1 (artículos seleccionados)
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Cantidad"].Value != null && row.Cells["Cantidad"].Value != DBNull.Value)
                {
                    int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["Precio"].Value);
                    int articuloID = Convert.ToInt32(row.Cells["ArticuloID"].Value);
                    int stock = Convert.ToInt32(row.Cells["Stock"].Value);

                    if (cantidad > stock)
                    {
                        MessageBox.Show("La cantidad solicitada excede el stock disponible.");
                        return;
                    }

                    // Actualizar stock en la base de datos
                    string updateStockQuery = "UPDATE Articulos SET Stock = Stock - @Cantidad WHERE ArticuloID = @ArticuloID";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand updateCommand = new SqlCommand(updateStockQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@Cantidad", cantidad);
                            updateCommand.Parameters.AddWithValue("@ArticuloID", articuloID);

                            updateCommand.ExecuteNonQuery();
                        }
                    }

                    // Sumar el monto de la venta
                    montoVentaTotal += precio * cantidad; // * TABLA DE 7 INTERESES PERO CUANDO ELIJO CREDITO QUE ME DE LA OPCION DE LAS CUOTAS Y EL VALOR TOTAL 

                    // Añadir el nombre del artículo vendido a la lista
                    string articuloNombre = row.Cells["Nombre"].Value.ToString();
                    articulosVendidos.Add($"{articuloNombre} x{cantidad}");
                }
            }

            string insertVentaQuery = @"
INSERT INTO Ventas (UsuarioID, ClienteID, MontoVenta, FechaVenta, MedioPago) 
VALUES (@UsuarioID, @ClienteID, @MontoVenta, @FechaVenta, @MedioPago);
SELECT SCOPE_IDENTITY();"; // Obtener el ID de la venta recién insertada.

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand insertVentaCommand = new SqlCommand(insertVentaQuery, connection))
                {
                    insertVentaCommand.Parameters.AddWithValue("@UsuarioID", UsuarioActual.Nombre); // Usuario actual
                    insertVentaCommand.Parameters.AddWithValue("@ClienteID", clienteID);
                    insertVentaCommand.Parameters.AddWithValue("@MontoVenta", montoVentaTotal);
                    insertVentaCommand.Parameters.AddWithValue("@FechaVenta", DateTime.Now);
                    insertVentaCommand.Parameters.AddWithValue("@MedioPago", medioPago); // Medio de pago seleccionado

                    int ventaID = Convert.ToInt32(insertVentaCommand.ExecuteScalar()); // Obtener el ID de la venta recién registrada.

                    // Registrar los artículos vendidos
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["Cantidad"].Value != null && row.Cells["Cantidad"].Value != DBNull.Value)
                        {
                            string insertArticuloVendidoQuery = @"
                    INSERT INTO ArticulosVendidos (VentaID, ArticuloID, Cantidad, Precio) 
                    VALUES (@VentaID, @ArticuloID, @Cantidad, @Precio)";

                            using (SqlCommand insertArticuloCommand = new SqlCommand(insertArticuloVendidoQuery, connection))
                            {
                                int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);
                                decimal precio = Convert.ToDecimal(row.Cells["Precio"].Value);
                                int articuloID = Convert.ToInt32(row.Cells["ArticuloID"].Value);

                                insertArticuloCommand.Parameters.AddWithValue("@VentaID", ventaID);
                                insertArticuloCommand.Parameters.AddWithValue("@ArticuloID", articuloID);
                                insertArticuloCommand.Parameters.AddWithValue("@Cantidad", cantidad);
                                insertArticuloCommand.Parameters.AddWithValue("@Precio", precio);

                                insertArticuloCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    // Crear el mensaje detallado con los artículos vendidos
                    string articulosVendidosTexto = string.Join(", ", articulosVendidos); // Concatenar los artículos con cantidades

                    // Registrar log
                    string logQuery = @"
            INSERT INTO Logs (UsuarioID, Accion, FechaHora) 
            VALUES (@UsuarioID, @Accion, @FechaHora)";

                    using (SqlCommand logCommand = new SqlCommand(logQuery, connection))
                    {
                        string accionLog = $"Venta registrada para el cliente {clienteNombre} {clienteApellido}: Se vendieron {articulosVendidos.Count} artículos ({articulosVendidosTexto}) por un total de {montoVentaTotal:C} usando el medio de pago: {medioPago}";

                        logCommand.Parameters.AddWithValue("@UsuarioID", UsuarioActual.Nombre); // Usuario actual
                        logCommand.Parameters.AddWithValue("@Accion", accionLog);
                        logCommand.Parameters.AddWithValue("@FechaHora", DateTime.Now);

                        logCommand.ExecuteNonQuery();
                    }
                }
            }
            MessageBox.Show("Venta realizada con éxito.", "Venta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Desvincula temporalmente el DataGridView
            dataGridView1.DataSource = null;

            // Limpiar las filas (esto funcionará sin el binding)
            dataGridView1.Rows.Clear();
        }
        // Limpiar el DataGridView después de realizar la venta

        private void button3_Click(object sender, EventArgs e) // Eliminar materias de dataGridView6
        {
            string searchTerm = textBox1.Text.Trim(); // Obtener el texto de búsqueda

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Por favor, ingrese un término de búsqueda.");
                return;
            }

            string query = @"
        SELECT ClienteID, Nombre, Apellido 
        FROM Clientes
        WHERE Nombre LIKE @SearchTerm OR Apellido LIKE @SearchTerm";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parámetro para evitar inyecciones SQL
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Asignar el resultado al DataGridView
                    dataGridView6.DataSource = dataTable;
                }
            }
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

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

