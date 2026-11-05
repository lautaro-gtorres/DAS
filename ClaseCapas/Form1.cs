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

namespace ClaseCapas
{
    public partial class Form1 : Form
    {
        BLL.Lotes lotesBLL = new BLL.Lotes();
        BLL.Ubicacion ubicacionBLL = new BLL.Ubicacion();
        BLL.Pago pagoBLL = new BLL.Pago();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                  int fa = 0;
            BE.Lotes lote = new BE.Lotes();
            lote.NombrePropietario = txtNombrePropietario.Text;
            lote.MedidaFrente = int.Parse(txtMedidaFrente.Text);
            lote.MedidaFondo = int.Parse(txtMedidaFondo.Text);
            lote.TotalMts2 = int.Parse(txtTotalMt2.Text);

            fa = lotesBLL.Alta(lote);

            if (fa != 0)
            {
                MessageBox.Show("Lote agregado correctamente");
                
            }
            else
            {
                MessageBox.Show("Error al agregar lote");
            }
                refresh();
            }
            catch (Exception)
            {

                throw;
            }
          

        }

        void refresh()
        {
            
            dataGridView1.DataSource = null;       
            dataGridView1.DataSource = lotesBLL.Listar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refresh();
            comboBox1.DataSource = null;
            comboBox1.DataSource = lotesBLL.Listar();
            comboBox1.DisplayMember = "nroLote";

            comboBox2.DataSource = null;
            comboBox2.DataSource = ubicacionBLL.Listar();
            comboBox2.DisplayMember = "idUbicacion";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BE.Lotes lote = (BE.Lotes)dataGridView1.SelectedRows[0].DataBoundItem;
            txtIdAlumno.Text = lote.NroLote.ToString();
            txtNombrePropietario.Text = lote.NombrePropietario;
            txtMedidaFrente.Text = lote.MedidaFrente.ToString();
            txtMedidaFondo.Text = lote.MedidaFondo.ToString();
            txtTotalMt2.Text = lote.TotalMts2.ToString();
        }
        

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int fa = 0;
            BE.Lotes lote = new BE.Lotes();
            lote.NroLote = int.Parse(txtIdAlumno.Text);
            lote.NombrePropietario = txtNombrePropietario.Text;
            lote.MedidaFrente = int.Parse(txtMedidaFrente.Text);
            lote.MedidaFondo = int.Parse(txtMedidaFondo.Text);
            lote.TotalMts2 = int.Parse(txtTotalMt2.Text);

            fa = lotesBLL.Modificar(lote);

            if (fa != 0)
            {
                MessageBox.Show("Lote modificado correctamente");

            }
            else
            {
                MessageBox.Show("Error al modificar lote");
            }
            refresh();
        }

        private void txtMedidaFrente_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtMedidaFrente.Text.Trim(), out var frente) &&
                int.TryParse(txtMedidaFondo.Text.Trim(), out var fondo))
            {
                txtTotalMt2.Text = (frente * fondo).ToString();
            }
            else
            {
                txtTotalMt2.Text = "";
            }
        }

        private void txtMedidaFondo_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtMedidaFrente.Text.Trim(), out var frente) &&
                int.TryParse(txtMedidaFondo.Text.Trim(), out var fondo))
            {
                txtTotalMt2.Text = (frente * fondo).ToString();
            }
            else
            {
                txtTotalMt2.Text = "";
            }
        }
       

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            BE.Lotes lote = (BE.Lotes)comboBox1.SelectedItem;
            textBox1.Text = lote.TotalMts2.ToString();
            if (int.TryParse(textBox1.Text.Trim(), out var totalMt2) &&
                int.TryParse(textBox2.Text.Trim(), out var valorMt2))
            {
                textBox3.Text = (totalMt2 * valorMt2).ToString();
            }
            else
            {
                textBox3.Text = "";
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            BE.Ubicacion ubicacion = (BE.Ubicacion)comboBox2.SelectedItem;
            textBox2.Text = ubicacion.ValorMt2.ToString();
            if (int.TryParse(textBox1.Text.Trim(), out var totalMt2) &&
                int.TryParse(textBox2.Text.Trim(), out var valorMt2))
            {
                textBox3.Text = (totalMt2 * valorMt2).ToString();
            }
            else
            {
                textBox3.Text = "";
            }
        }

        private void btnPago_Click(object sender, EventArgs e)
        {
            try
            {
                int fa = 0;
                BE.Pago pago = new BE.Pago();
                pago.NroLote = int.Parse(comboBox1.Text);
                pago.Ubicacion = comboBox2.Text;
                pago.Mt2Total = int.Parse(textBox1.Text);
                pago.PrecioMt2 = int.Parse(textBox2.Text);
                pago.Total = int.Parse(textBox3.Text);

                fa = pagoBLL.Alta(pago);

                if (fa != 0)
                {
                    MessageBox.Show("Pago registrado correctamente");
                }
                else
                {
                    MessageBox.Show("Error al registrar pago");
                }
                refresh();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnListaPagados_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join(", ", pagoBLL.ListaPagados()));
        }

        private void btnPromedioTotal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("El promedio total de los pagos es: " + pagoBLL.PromedioTotal());
        }
    }
}
