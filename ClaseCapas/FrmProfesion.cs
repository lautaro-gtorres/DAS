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
    public partial class FrmProfesion : Form
    {
        BLL.Profesion profesionBLL = new BLL.Profesion();
        public FrmProfesion()
        {
            InitializeComponent();
        }
        void refresh()
        {

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = profesionBLL.Listar();
        }
        private void btnAgregarProfesion_Click(object sender, EventArgs e)
        {
            BE.Profesion profesion = new BE.Profesion();
            profesion.DescProfesion = txtDescProfesion.Text;
            int fa = profesionBLL.Alta(profesion);
            if (fa > 0)
            {
                MessageBox.Show("Profesión agregada correctamente");
                txtDescProfesion.Clear();
            }
            else
            {
                MessageBox.Show("Error al agregar la profesión");
            }
            refresh();
        }

        private void FrmProfesion_Load(object sender, EventArgs e)
        {
            refresh();
        }

        private void btnEditarProfesion_Click(object sender, EventArgs e)
        {
            BE.Profesion profesion = new BE.Profesion();
            profesion.IdProfesion = Convert.ToInt32(txtIdProfesion.Text);
            profesion.DescProfesion = txtDescProfesion.Text;
            int fa = profesionBLL.Modificar(profesion);
            if (fa > 0)
            {
                MessageBox.Show("Profesión modificada correctamente");
                txtDescProfesion.Clear();
                txtIdProfesion.Clear();
            }
            else
            {
                MessageBox.Show("Error al modificar la profesión");
            }
            refresh();
        }

        private void btnEliminarProfesion_Click(object sender, EventArgs e)
        {
            BE.Profesion profesion = new BE.Profesion();
            int idProfesion = Convert.ToInt32(txtIdProfesion.Text);
            int fa = profesionBLL.Baja(idProfesion);
            if (fa > 0)
            {
                MessageBox.Show("Profesión eliminada correctamente");
                txtDescProfesion.Clear();
                txtIdProfesion.Clear();
            }
            else
            {
                MessageBox.Show("Error al eliminar la profesión");
            }
            refresh();
        }
    }
}
