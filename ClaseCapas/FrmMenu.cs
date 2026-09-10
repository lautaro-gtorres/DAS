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
    public partial class FrmMenu : Form
    {
        BLL.Persona alumnoBLL = new BLL.Persona();
        public FrmMenu()
        {
            InitializeComponent();
        }

        //private void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //          int fa = 0;
        //    BE.Persona alumno = new BE.Persona();
        //    alumno.Nombre = txtNombreAlumno.Text;
        //    alumno.Apellido = txtApellidoAlumno.Text;
        //    alumno.Edad = int.Parse(txtEdadAlumno.Text);
        //    alumno.Genero = txtGeneroAlumno.Text;

        //    fa = alumnoBLL.Alta(alumno);

        //    if (fa != 0)
        //    {
        //        MessageBox.Show("Alumno agregado correctamente");
                
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error al agregar alumno");
        //    }
        //        refresh();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
          

        //}

        void AbrirHijo(Form hijo)
        {
            hijo.MdiParent = this;
            hijo.Show();
        }

        //private void personaToolStripMenuItem_Click(object sender, EventArgs e)


        //private void nacionalidadToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    AbrirHijo(new FrmNacionalidadABM());
        //}

        //private void profesionesToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    AbrirHijo(new FrmProfesionABM());
        //}

        void refresh()
        {
            
            dataGridView1.DataSource = null;       
            dataGridView1.DataSource = alumnoBLL.Listar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //refresh();
        }

        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    BE.Persona alumn = (BE.Persona)dataGridView1.SelectedRows[0].DataBoundItem;
        //    txtIdAlumno.Text = alumn.IdAlumno.ToString();
        //    txtNombreAlumno.Text = alumn.Nombre;
        //    txtApellidoAlumno.Text = alumn.Apellido;
        //    txtEdadAlumno.Text = alumn.Edad.ToString();
        //    txtGeneroAlumno.Text = alumn.Genero;
        //}

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtIdAlumno.Text != "")
            {
                int idAlumno = int.Parse(txtIdAlumno.Text);
                int fa = alumnoBLL.Baja(idAlumno);
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

        //private void btnEditar_Click(object sender, EventArgs e)
        //{
        //    int fa = 0;
        //    BE.Persona alumno = new BE.Persona();
        //    alumno.IdAlumno = int.Parse(txtIdAlumno.Text);
        //    alumno.Nombre = txtNombreAlumno.Text;
        //    alumno.Apellido = txtApellidoAlumno.Text;
        //    alumno.Edad = int.Parse(txtEdadAlumno.Text);
        //    alumno.Genero = txtGeneroAlumno.Text;

        //    fa = alumnoBLL.Modificar(alumno);

        //    if (fa != 0)
        //    {
        //        MessageBox.Show("Alumno modificado correctamente");

        //    }
        //    else
        //    {
        //        MessageBox.Show("Error al modificar alumno");
        //    }
        //    refresh();
        //}

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

        private void personaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                AbrirHijo(new FrmPersona());
            }
        }

        private void nacionalidadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmNacionalidad());
        }

        private void profesionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmProfesion());
        }
               

        private void buscarPersonaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void resumenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
