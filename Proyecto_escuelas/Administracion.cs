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
    public partial class Administracion : Form
    {
        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public Administracion()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void Administracion_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Cargar Vendedores
                    SqlDataAdapter daVendedores = new SqlDataAdapter("SELECT UsuarioID, NombreUsuario, FechaAlta FROM Usuarios WHERE Rol = 'Vendedor'", connection);
                    DataTable dtVendedores = new DataTable();
                    daVendedores.Fill(dtVendedores);
                    dataGridView1.DataSource = dtVendedores;

                    // Cargar Administradores
                    SqlDataAdapter daAdministradores = new SqlDataAdapter("SELECT UsuarioID, NombreUsuario, FechaAlta FROM Usuarios WHERE Rol = 'Admin'", connection);
                    DataTable dtAdministradores = new DataTable();
                    daAdministradores.Fill(dtAdministradores);
                    dataGridView2.DataSource = dtAdministradores;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombreUsuario = textBox1.Text.Trim();
            string contrasena = textBox2.Text.Trim();
            string rol = checkBox1.Checked ? "Admin" : "Vendedor";

            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Usuarios (NombreUsuario, Contrasena, Rol) VALUES (@NombreUsuario, @Contrasena, @Rol)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        command.Parameters.AddWithValue("@Contrasena", contrasena);
                        command.Parameters.AddWithValue("@Rol", rol);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Usuario creado exitosamente.");
                    }
                }

                // Recargar los DataGridView
                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear usuario: {ex.Message}");
            }
        }

        private void LimpiarCampos()
        {
            textBox1.Clear();
            textBox2.Clear();
            checkBox1.Checked = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // Tab de Vendedores
            {
                CambiarRol(dataGridView1, "Admin");
            }
            else if (tabControl1.SelectedIndex == 1) // Tab de Administradores
            {
                CambiarRol(dataGridView2, "Vendedor");
            }
        }
        private void CambiarRol(DataGridView dataGridView, string nuevoRol)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario.");
                return;
            }

            int usuarioID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["UsuarioID"].Value);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Usuarios SET Rol = @Rol WHERE UsuarioID = @UsuarioID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Rol", nuevoRol);
                        command.Parameters.AddWithValue("@UsuarioID", usuarioID);

                        command.ExecuteNonQuery();
                        MessageBox.Show($"El rol del usuario se cambió a {nuevoRol}.");
                    }
                }

                // Recargar los DataGridView
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar el rol: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // Tab de Vendedores
            {
                CambiarContrasena(dataGridView1);
            }
            else if (tabControl1.SelectedIndex == 1) // Tab de Administradores
            {
                CambiarContrasena(dataGridView2);
            }
        }
        private void CambiarContrasena(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario.");
                return;
            }

            // Obtener el UsuarioID del usuario seleccionado
            int usuarioID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["UsuarioID"].Value);

            // Solicitar nueva contraseña
            string nuevaContrasena = Microsoft.VisualBasic.Interaction.InputBox(
                "Ingrese la nueva contraseña para el usuario seleccionado:",
                "Cambiar Contraseña",
                ""
            );

            if (string.IsNullOrEmpty(nuevaContrasena))
            {
                MessageBox.Show("La contraseña no puede estar vacía.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Usuarios SET Contrasena = @Contrasena WHERE UsuarioID = @UsuarioID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Contrasena", nuevaContrasena);
                        command.Parameters.AddWithValue("@UsuarioID", usuarioID);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Contraseña cambiada exitosamente.");
                    }
                }

                // Recargar los DataGridView para reflejar los cambios
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar la contraseña: {ex.Message}");
            }
        }

        private void Inicio_Click(object sender, EventArgs e)
        {
            Form1 inicio = new Form1(UsuarioActual.Nombre, UsuarioActual.Rol); // Cambia "UsuarioActual" y "Vendedor" según corresponda
            inicio.Show();
            this.Hide();

        }
    }
}