using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    public partial class FrmBuscarPersona : Form
    {
        BLL.Persona personaBLL = new BLL.Persona();
        BLL.Nacionalidad nacionalidadBLL = new BLL.Nacionalidad();
        BLL.Profesion profesionBLL = new BLL.Profesion();

        public FrmBuscarPersona()
        {
            InitializeComponent();
        }

        private void FrmBuscarPersona_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = null;
            comboBox1.DataSource = personaBLL.Listar();
           comboBox1.DisplayMember = "IdPersona";
            comboBox1.ValueMember = "IdPersona";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BE.Persona persona = comboBox1.SelectedItem as BE.Persona;
            if (persona == null) return;

            txtIdPersona.Text = persona.IdPersona.ToString();
            txtNombrePersona.Text = persona.Nombre;
            txtApellidoPersona.Text = persona.Apellido;
            txtEdadPersona.Text = persona.Edad.ToString();
            txtSexoPersona.Text = persona.Sexo?.Trim();

            txtNacionalidad.Text = BuscarNombreNacionalidad(persona.Idnacionalidad);
            txtProfesion.Text = BuscarNombreProfesion(persona.IdProfesion);
        }

        string BuscarNombreNacionalidad(int id)
        {
            List<BE.Nacionalidad> lista = nacionalidadBLL.Listar();
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].IdNacionalidad == id)
                    return lista[i].DescNacionalidad;
            }
            return "";
        }

        string BuscarNombreProfesion(int id)
        {
            List<BE.Profesion> lista = profesionBLL.Listar();
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].IdProfesion == id)
                    return lista[i].DescProfesion;
            }
            return "";
        }
    }
}
