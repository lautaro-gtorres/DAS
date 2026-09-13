using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    public partial class FrmNacionalidad : Form
    {
        BLL.Nacionalidad nacionalidadBLL = new BLL.Nacionalidad();
        public FrmNacionalidad()
        {
            InitializeComponent();
        }
        void refresh()
        {

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = nacionalidadBLL.Listar();
        }

        private void btnAgregarNacionalidad_Click(object sender, EventArgs e)
        {
            try
            {
                int fa = 0;
                BE.Nacionalidad nacionalidad = new BE.Nacionalidad();
                nacionalidad.DescNacionalidad = txtDescNacionalidad.Text;
                fa = nacionalidadBLL.Alta(nacionalidad);
                if (fa > 0)
                {
                    MessageBox.Show("Nacionalidad agregada correctamente");
                    txtDescNacionalidad.Clear();
                }
                else
                {
                    MessageBox.Show("Error al agregar la nacionalidad");
                }
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditarNacionalidad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdNacionalidad.Text))
            {
                MessageBox.Show("Seleccione una nacionalidad de la lista.");
                return;
            }
            try
            {
                int fa = 0;
                BE.Nacionalidad nacionalidad = new BE.Nacionalidad();
                nacionalidad.IdNacionalidad = Convert.ToInt32(txtIdNacionalidad.Text);
                nacionalidad.DescNacionalidad = txtDescNacionalidad.Text;
                fa = nacionalidadBLL.Modificar(nacionalidad);
                if (fa > 0)
                {
                    MessageBox.Show("Nacionalidad modificada correctamente");
                    txtDescNacionalidad.Clear();
                }
                else
                {
                    MessageBox.Show("Error al modificar la nacionalidad");
                }
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEliminarNacionalidad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdNacionalidad.Text))
            {
                MessageBox.Show("Seleccione una nacionalidad de la lista.");
                return;
            }
            int fa = 0;
            int idNacionalidad = Convert.ToInt32(txtIdNacionalidad.Text);
            fa = nacionalidadBLL.Baja(idNacionalidad);
            if (fa > 0)
            {
                MessageBox.Show("Nacionalidad eliminada correctamente");
                txtDescNacionalidad.Clear();
            }
            else
            {
                MessageBox.Show("Error al eliminar la nacionalidad");
            }
            refresh();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BE.Nacionalidad nacionalidad = (BE.Nacionalidad)dataGridView1.SelectedRows[0].DataBoundItem;
            txtIdNacionalidad.Text = nacionalidad.IdNacionalidad.ToString();
            txtDescNacionalidad.Text = nacionalidad.DescNacionalidad;            
        }

        private void FrmNacionalidad_Load(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
