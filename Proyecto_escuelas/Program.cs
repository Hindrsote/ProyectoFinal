using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class UsuarioActual
{
    public static string Nombre { get; set; }
    public static string Rol { get; set; }
}

public class ArticuloSeleccionado
{
    public int ArticuloID { get; set; }
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public int Stock { get; set; }
}

namespace Proyecto_escuelas
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Form1 mainForm = new Form1(loginForm.NombreUsuario, loginForm.RolUsuario);
                    Application.Run(mainForm);
                }
            }
        }
    }
}
