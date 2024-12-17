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
    public partial class Ventacarga : Form
    {

        private string connectionString = "Server=DESKTOP-2MVFTUI;Database=TiendaDB;Trusted_Connection=True;TrustServerCertificate=True;";
        private List<ArticuloSeleccionado> articulosSeleccionados;
        public Ventacarga(List<ArticuloSeleccionado> articulosSeleccionados)
        {
            InitializeComponent();
            this.articulosSeleccionados = articulosSeleccionados;
        }


        private void BuscarArticulos(string nombreArticulo)
        {
            string query = "SELECT ArticuloID, Nombre, Precio, Stock FROM Articulos WHERE Nombre LIKE @NombreArticulo AND Visible = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@NombreArticulo", "%" + nombreArticulo + "%");

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;

                // Asegúrate de que ArticuloID esté en las columnas y ocúltalo si es necesario
                if (dataGridView1.Columns.Contains("ArticuloID"))
                {
                    dataGridView1.Columns["ArticuloID"].Visible = false;
                }

                if (dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron artículos con ese nombre.");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string nombreArticulo = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(nombreArticulo))
            {
                MessageBox.Show("Por favor, ingrese el nombre del artículo.");
                return;
            }

            BuscarArticulos(nombreArticulo);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un artículo.");
                return;
            }

            if (!int.TryParse(textBox2.Text.Trim(), out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Por favor, ingrese una cantidad válida.");
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int articuloID = Convert.ToInt32(selectedRow.Cells["ArticuloID"].Value); // Capturar el ID
            string nombre = selectedRow.Cells["Nombre"].Value.ToString();
            decimal precio = Convert.ToDecimal(selectedRow.Cells["Precio"].Value);
            int stock = Convert.ToInt32(selectedRow.Cells["Stock"].Value);

            if (cantidad > stock)
            {
                MessageBox.Show("La cantidad solicitada excede el stock disponible.");
                return;
            }

            ArticuloSeleccionado articulo = new ArticuloSeleccionado
            {
                ArticuloID = articuloID, // Guardar el ID
                Nombre = nombre,
                Precio = precio,
                Cantidad = cantidad,
                Stock = stock
            };

            articulosSeleccionados.Add(articulo);
            MessageBox.Show("Artículo añadido correctamente.");
            this.Close();
        }
    }
}
