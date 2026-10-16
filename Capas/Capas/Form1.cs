using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        BLL.Alumno gestorAlumno = new BLL.Alumno();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            CargarGrilla();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int fa = 0;

            BE.Alumno alumno = new BE.Alumno();

            alumno.IDAlumno = int.Parse(txtIdAlumno.Text);
            alumno.Nombre = txtNombre.Text;
            alumno.Apellido = txtApellido.Text;
            alumno.Edad = int.Parse(txtEdad.Text);
            alumno.Genero = txtGenero.Text;

            fa = gestorAlumno.AltaAlumno(alumno);

            if (fa == 1)
            {
                MessageBox.Show("Alumno agregado con éxito");

                LimpiarTextBox();
                //CargarGrilla();
            }
            else
            {
                MessageBox.Show("Ocurrió un error");
            }
        }

        private void CargarGrilla()
        {
            dataGridView1.DataSource = gestorAlumno.ListarAlumno();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarGrilla();

            MessageBox.Show("Datos actualizados correctamente");
        }

        private void LimpiarTextBox()
        {
            txtIdAlumno.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtEdad.Clear();
            txtGenero.Clear();
            //--
            txtIdAlumno.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int idAlumno = Convert.ToInt32(
                    dataGridView1.SelectedRows[0].Cells["idAlumno"].Value
                );

                DialogResult respuesta = MessageBox.Show(
                    "¿Está seguro que desea eliminar este alumno?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta == DialogResult.Yes)
                {
                    int fa = gestorAlumno.BajaAlumno(idAlumno);

                    if (fa == 1)
                    {
                        MessageBox.Show("Alumno eliminado con éxito");

                        LimpiarTextBox();
                        CargarGrilla();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el alumno");
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un alumno para eliminar");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                txtIdAlumno.Text = fila.Cells["idAlumno"].Value.ToString();
                txtNombre.Text = fila.Cells["nombre"].Value.ToString();
                txtApellido.Text = fila.Cells["apellido"].Value.ToString();
                txtEdad.Text = fila.Cells["edad"].Value.ToString();
                txtGenero.Text = fila.Cells["genero"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdAlumno.Text))
            {
                MessageBox.Show("Seleccione un alumno para modificar");
                return;
            }

            BE.Alumno alumno = new BE.Alumno();

            alumno.IDAlumno = int.Parse(txtIdAlumno.Text);
            alumno.Nombre = txtNombre.Text;
            alumno.Apellido = txtApellido.Text;
            alumno.Edad = int.Parse(txtEdad.Text);
            alumno.Genero = txtGenero.Text;

            int fa = gestorAlumno.EditarAlumno(alumno);

            if (fa == 1)
            {
                MessageBox.Show("Alumno actualizado con éxito");

                LimpiarTextBox();
                CargarGrilla();
            }
            else
            {
                MessageBox.Show("Ocurrió un error al actualizar");
            }
        }
    }
}
