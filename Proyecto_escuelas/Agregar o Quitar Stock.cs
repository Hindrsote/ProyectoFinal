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
using static Proyecto_escuelas.Form1;

namespace Proyecto_escuelas
{
    public partial class Agregar_o_Quitar_Stock : Form
    {
        public Agregar_o_Quitar_Stock()
        {
            InitializeComponent();
            LoadData();
        }

        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // Método para cargar los datos en dataGridView1
        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ArticuloID, Nombre, Precio, Stock FROM Articulos WHERE Visible = 1";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Agregar_o_Quitar_Stock_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un artículo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox1.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener datos del artículo seleccionado
            int articuloId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ArticuloID"].Value);
            string nombreArticulo = dataGridView1.SelectedRows[0].Cells["Nombre"].Value.ToString();
            string descripcion = textBox3.Text; // Descripción adicional

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Actualizar el stock del artículo
                            string updateStockQuery = "UPDATE Articulos SET Stock = Stock + @Cantidad WHERE ArticuloID = @ArticuloID";
                            using (SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, connection, transaction))
                            {
                                updateStockCommand.Parameters.AddWithValue("@Cantidad", cantidad);
                                updateStockCommand.Parameters.AddWithValue("@ArticuloID", articuloId);
                                updateStockCommand.ExecuteNonQuery();
                            }

                            // 2. Registrar en la tabla ActividadUsuarios
                            string actividadUsuariosQuery = @"
                        INSERT INTO ActividadUsuarios (UsuarioID, Descripcion) 
                        VALUES (@UsuarioID, @Descripcion)";
                            using (SqlCommand actividadCommand = new SqlCommand(actividadUsuariosQuery, connection, transaction))
                            {
                                actividadCommand.Parameters.AddWithValue("@UsuarioID", UsuarioActual.Nombre); // Usamos el nombre del usuario actual
                                actividadCommand.Parameters.AddWithValue("@Descripcion", $"Se agregó {cantidad} unidades al artículo '{nombreArticulo}'. {descripcion}");
                                actividadCommand.ExecuteNonQuery();
                            }

                            // 3. Registrar en la tabla Logs
                            string logsQuery = @"
                        INSERT INTO Logs (UsuarioID, Accion, IP) 
                        VALUES (@UsuarioID, @Accion, @IP)";
                            using (SqlCommand logsCommand = new SqlCommand(logsQuery, connection, transaction))
                            {
                                logsCommand.Parameters.AddWithValue("@UsuarioID", UsuarioActual.Nombre); // Usamos el nombre del usuario actual
                                logsCommand.Parameters.AddWithValue("@Accion", $"Se agregó stock al artículo '{nombreArticulo}'. {descripcion}");
                                logsCommand.Parameters.AddWithValue("@IP", ObtenerIP()); // Método para obtener la IP actual
                                logsCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show("Stock actualizado y acción registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Recargar los datos en dataGridView1
                            LoadData();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerIP()
        {
            try
            {
                return System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())
                       .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                       .ToString() ?? "Desconocido";
            }
            catch
            {
                return "Desconocido";
            }
        }

        // Método para inicializar el formulario
        private void AgregarOQuitarStock_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void Inicio_Click(object sender, EventArgs e)
        {
            Form1 inicio = new Form1(UsuarioActual.Nombre, UsuarioActual.Rol); // Cambia "UsuarioActual" y "Vendedor" según corresponda
            inicio.Show();
            this.Hide();
        }
    }
}
