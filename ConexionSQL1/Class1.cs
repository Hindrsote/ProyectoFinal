using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Punto_de_Venta_Datos
{
    public class Conexion
    {
        private string Base;
        private string Servidor;
        private string Usuario;
        private string Clave;
        private static Conexion Con = null;

        private Conexion()
        {
            this.Base = "tp_lab4";
            this.Servidor = "DESKTOP-2MVFTUI";
            this.Usuario = "";
            this.Clave = "";
        }
        public SqlConnection crear_conexion()
        {
            SqlConnection cadena = new SqlConnection();
            try 
            {
                cadena.ConnectionString = "Server=" + this.Servidor +
                                           "; Database=" + this.Base +
                                            "; User id=" + this.Usuario +
                                            "; password=" + this.Clave
                    ;
            }
            catch (Exception ex)
            {

                cadena = null;
                throw ex;
            }
            return cadena;
        }
        public static Conexion getInstancia()
        {
            if (Con == null)
            {
                Con = new Conexion();
            }
            return Con;
        }
    }
}