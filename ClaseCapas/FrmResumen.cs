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
    public partial class FrmResumen : Form
    {
        public FrmResumen()
        {
            InitializeComponent();
        }

        BLL.Resumen resumenBLL = new BLL.Resumen();
        BLL.Nacionalidad nacionalidadBLL = new BLL.Nacionalidad();
        BLL.Profesion profesionBLL = new BLL.Profesion();

        List<BE.ResumenNacionalidad> porNacionalidad;
        List<BE.ResumenProfesion> porProfesion;

        private void FrmResumen_Load(object sender, EventArgs e)
        {
            var general = resumenBLL.ObtenerResumenGeneral();
            porNacionalidad = resumenBLL.ObtenerResumenPorNacionalidad();
            porProfesion = resumenBLL.ObtenerResumenPorProfesion();

            string nacionalidadConMas = "-";
            string nacionalidadConMenos = "-";
            bool yaEncontroLaPrimera = false;

            for (int i = 0; i < porNacionalidad.Count; i++)
            {
                BE.ResumenNacionalidad n = porNacionalidad[i];
                if (n.Cantidad > 0)
                {
                    if (!yaEncontroLaPrimera)
                    {
                        nacionalidadConMas = n.DescNacionalidad;
                        yaEncontroLaPrimera = true;
                    }
                    nacionalidadConMenos = n.DescNacionalidad;
                }
            }

            label1.Text =
                "Resumen general\n" +
                "Cantidad de personas: " + general.Cantidad + "\n" +
                "Promedio de edad: " + general.PromedioEdad.ToString("0.00") + "\n" +
                "Edad mínima: " + general.EdadMinima + "\n" +
                "Edad máxima: " + general.EdadMaxima + "\n" +
                "Nacionalidad con más personas: " + nacionalidadConMas + "\n" +
                "Nacionalidad con menos personas: " + nacionalidadConMenos;

            List<BE.Nacionalidad> nacionalidades = new List<BE.Nacionalidad>();
            nacionalidades.Add(new BE.Nacionalidad { IdNacionalidad = 0, DescNacionalidad = "(Todas)" });
            nacionalidades.AddRange(nacionalidadBLL.Listar());
            comboBox1.DataSource = nacionalidades;
            comboBox1.DisplayMember = "DescNacionalidad";
            comboBox1.ValueMember = "IdNacionalidad";

            List<BE.Profesion> profesiones = new List<BE.Profesion>();
            profesiones.Add(new BE.Profesion { IdProfesion = 0, DescProfesion = "(Todas)" });
            profesiones.AddRange(profesionBLL.Listar());
            comboBox2.DataSource = profesiones;
            comboBox2.DisplayMember = "DescProfesion";
            comboBox2.ValueMember = "IdProfesion";

            MostrarPorNacionalidad(0);
            MostrarPorProfesion(0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BE.Nacionalidad seleccion = comboBox1.SelectedItem as BE.Nacionalidad;
            if (seleccion == null) return;
            MostrarPorNacionalidad(seleccion.IdNacionalidad);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            BE.Profesion seleccion = comboBox2.SelectedItem as BE.Profesion;
            if (seleccion == null) return;
            MostrarPorProfesion(seleccion.IdProfesion);
        }

        void MostrarPorNacionalidad(int idFiltro)
        {
            string texto = "Resumen por nacionalidad\n";
            for (int i = 0; i < porNacionalidad.Count; i++)
            {
                BE.ResumenNacionalidad n = porNacionalidad[i];
                if (idFiltro == 0 || n.IdNacionalidad == idFiltro)
                    texto += n.DescNacionalidad + ": " + n.Cantidad + " personas, promedio " + n.PromedioEdad.ToString("0.00") + " años\n";
            }
            label2.Text = texto;
        }

        void MostrarPorProfesion(int idFiltro)
        {
            string texto = "Resumen por profesión\n";
            for (int i = 0; i < porProfesion.Count; i++)
            {
                BE.ResumenProfesion p = porProfesion[i];
                if (idFiltro == 0 || p.IdProfesion == idFiltro)
                    texto += p.DescProfesion + ": " + p.Cantidad + " personas, promedio " + p.PromedioEdad.ToString("0.00") + " años\n";
            }
            label3.Text = texto;
        }
    }
}
