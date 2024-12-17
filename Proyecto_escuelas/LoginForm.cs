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
    public partial class LoginForm : Form
    {
        public string NombreUsuario { get; private set; }
        public string RolUsuario { get; private set; }

        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text.Trim();
            string contrasena = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Rol FROM Usuarios WHERE NombreUsuario = @Usuario AND Contrasena = @Contrasena";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Usuario", usuario);
                        command.Parameters.AddWithValue("@Contrasena", contrasena);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            RolUsuario = result.ToString();
                            NombreUsuario = usuario;

                            // Asignar los valores a las propiedades estáticas
                            UsuarioActual.Nombre = NombreUsuario;
                            UsuarioActual.Rol = RolUsuario;

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}");
            }
        }
    }
}