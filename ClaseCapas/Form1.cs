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
        BLL.Alumno alumnoBLL = new BLL.Alumno();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                  int fa = 0;
            BE.Alumno alumno = new BE.Alumno();
            alumno.Nombre = txtNombreAlumno.Text;
            alumno.Apellido = txtApellidoAlumno.Text;
            alumno.Edad = int.Parse(txtEdadAlumno.Text);
            alumno.Genero = txtGeneroAlumno.Text;

            fa = alumnoBLL.AltaAlumno(alumno);

            if (fa != 0)
            {
                MessageBox.Show("Alumno agregado correctamente");
                
            }
            else
            {
                MessageBox.Show("Error al agregar alumno");
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
            dataGridView1.DataSource = alumnoBLL.ListarAlumnos(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BE.Alumno alumn = (BE.Alumno)dataGridView1.SelectedRows[0].DataBoundItem;
            txtIdAlumno.Text = alumn.IdAlumno.ToString();
            txtNombreAlumno.Text = alumn.Nombre;
            txtApellidoAlumno.Text = alumn.Apellido;
            txtEdadAlumno.Text = alumn.Edad.ToString();
            txtGeneroAlumno.Text = alumn.Genero;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtIdAlumno.Text != "")
            {
                int idAlumno = int.Parse(txtIdAlumno.Text);
                int fa = alumnoBLL.BajaAlumno(idAlumno);
                if (fa != 0)
                {
                    MessageBox.Show("Alumno " + idAlumno + " eliminado correctamente");
                    refresh();
                }
                else
                {
                    MessageBox.Show("Error al eliminar alumno");
                }
            }
            else
            {
                MessageBox.Show("Seleccione un alumno de la lista para eliminar");
            }
            refresh();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int fa = 0;
            BE.Alumno alumno = new BE.Alumno();
            alumno.IdAlumno = int.Parse(txtIdAlumno.Text);
            alumno.Nombre = txtNombreAlumno.Text;
            alumno.Apellido = txtApellidoAlumno.Text;
            alumno.Edad = int.Parse(txtEdadAlumno.Text);
            alumno.Genero = txtGeneroAlumno.Text;

            fa = alumnoBLL.ModificarAlumno(alumno);

            if (fa != 0)
            {
                MessageBox.Show("Alumno modificado correctamente");

            }
            else
            {
                MessageBox.Show("Error al modificar alumno");
            }
            refresh();
        }

        public void LimpiarCampos()
        {
            txtIdAlumno.Text = "";
            txtNombreAlumno.Text = "";
            txtApellidoAlumno.Text = "";
            txtEdadAlumno.Text = "";
            txtGeneroAlumno.Text = "";
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}
