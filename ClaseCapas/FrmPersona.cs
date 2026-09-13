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
    public partial class FrmPersona : Form
    {
        BLL.Persona personaBLL = new BLL.Persona();
        BLL.Nacionalidad nacionalidadBLL = new BLL.Nacionalidad();
        BLL.Profesion profesionBLL = new BLL.Profesion();
        public FrmPersona()
        {
            InitializeComponent();
        }
        void refresh()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = personaBLL.Listar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                int fa = 0;
                BE.Persona persona = new BE.Persona();
                persona.Nombre = txtNombrePersona.Text;
                persona.Apellido = txtApellidoPersona.Text;
                persona.Edad = int.TryParse(txtEdadPersona.Text, out int edad) ? edad : 0;
                persona.Sexo = txtSexoPersona.Text;
                persona.Idnacionalidad = Convert.ToInt32(cmbNacionalidad.SelectedValue);
                persona.IdProfesion = Convert.ToInt32(cmbProfesion.SelectedValue);
                fa = personaBLL.Alta(persona);
                if (fa != 0)
                {
                    MessageBox.Show("Persona agregada correctamente");
                }
                else
                {
                    MessageBox.Show("Error al agregar persona");
                }
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmPersona_Load(object sender, EventArgs e)
        {
            cmbNacionalidad.DataSource = null;
            cmbNacionalidad.DataSource = nacionalidadBLL.Listar();
            cmbNacionalidad.DisplayMember = "descNacionalidad";
            cmbNacionalidad.ValueMember = "idNacionalidad";
            cmbProfesion.DataSource = null;
            cmbProfesion.DataSource = profesionBLL.Listar();
            cmbProfesion.DisplayMember = "descProfesion";
            cmbProfesion.ValueMember = "idProfesion";
            refresh();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            BE.Persona persona = (BE.Persona)dataGridView1.SelectedRows[0].DataBoundItem;
            txtIdPersona.Text = persona.IdPersona.ToString();
            txtNombrePersona.Text = persona.Nombre;
            txtApellidoPersona.Text = persona.Apellido;
            txtEdadPersona.Text = persona.Edad.ToString();
            txtSexoPersona.Text = persona.Sexo;
            cmbNacionalidad.SelectedValue = persona.Idnacionalidad;
            cmbProfesion.SelectedValue = persona.IdProfesion;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdPersona.Text))
            {
                MessageBox.Show("Seleccione una persona de la lista.");
                return;
            }
            try
            {
                int fa = 0;
                BE.Persona persona = new BE.Persona();
                persona.IdPersona = int.Parse(txtIdPersona.Text);
                persona.Nombre = txtNombrePersona.Text;
                persona.Apellido = txtApellidoPersona.Text;
                persona.Edad = int.TryParse(txtEdadPersona.Text, out int edad) ? edad : 0;
                persona.Sexo = txtSexoPersona.Text;
                persona.Idnacionalidad = Convert.ToInt32(cmbNacionalidad.SelectedValue);
                persona.IdProfesion = Convert.ToInt32(cmbProfesion.SelectedValue);
                fa = personaBLL.Modificar(persona);
                if (fa != 0)
                {
                    MessageBox.Show("Persona modificada correctamente");
                }
                else
                {
                    MessageBox.Show("Error al modificar persona");
                }
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdPersona.Text))
            {
                MessageBox.Show("Seleccione una persona de la lista.");
                return;
            }
            int fa = 0;
            BE.Persona persona = new BE.Persona();
            persona.IdPersona = int.Parse(txtIdPersona.Text);
            fa = personaBLL.Baja(persona.IdPersona);
            if (fa != 0)
            {
                MessageBox.Show("Persona eliminada correctamente");
            }
            else
            {
                MessageBox.Show("Error al eliminar persona");
            }
            refresh();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombrePersona.Text = "";
            txtApellidoPersona.Text = "";
            txtEdadPersona.Text = "";
            txtSexoPersona.Text = "";
            txtIdPersona.Text = "";
            refresh();
        }
    }
}
