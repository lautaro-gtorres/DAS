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
            }
            catch (Exception)
            {

                throw;
            }
          

        }

        void refresh()
        {
            
            dataGridView1.DataSource = null;
            List<BE.Alumno> alumnos = new List<BE.Alumno>();
            alumnos = alumnoBLL.ListarAlumnos();
            dataGridView1.DataSource = alumnos; 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
