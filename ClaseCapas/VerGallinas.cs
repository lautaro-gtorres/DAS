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
    /// <summary>
    /// Listado de gallinas activas. Actualizar abre la gallina elegida para modificarla;
    /// Eliminar la da de baja (baja lógica).
    /// </summary>
    public partial class VerGallinas : Form
    {
        BLL.BLLGallina bll = new BLL.BLLGallina();

        public VerGallinas()
        {
            InitializeComponent();
        }

        private void VerGallinas_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvGallinas);
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                Pantalla.Llenar(dgvGallinas, bll.Listar());
                Pantalla.Ocultar(dgvGallinas, "Activo");
                Pantalla.Titulo(dgvGallinas, "CodGallina", "Código");
                Pantalla.Titulo(dgvGallinas, "TipoGallina", "Tipo");
                Pantalla.Titulo(dgvGallinas, "FechaNacimiento", "Nacimiento");
                Pantalla.Titulo(dgvGallinas, "NroCorral", "Corral");
                dgvGallinas.Columns["FechaNacimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Gallinas");
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BE.Gallina gallina = (BE.Gallina)Pantalla.Seleccionado(dgvGallinas);
            if (gallina == null)
            {
                Pantalla.Avisar("Seleccione una gallina de la lista.", "Gallinas");
                return;
            }
            CargarGallina editar = new CargarGallina(gallina);
            if (editar.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            BE.Gallina gallina = (BE.Gallina)Pantalla.Seleccionado(dgvGallinas);
            if (gallina == null)
            {
                Pantalla.Avisar("Seleccione una gallina de la lista.", "Gallinas");
                return;
            }
            if (!Pantalla.Confirmar("¿Dar de baja la gallina " + gallina.CodGallina + "?", "Gallinas"))
                return;
            try
            {
                bll.Baja(gallina.IdGallina);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Gallinas");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Gallinas gallinas = new Gallinas();
            gallinas.Show();
            this.Close();
        }
    }
}
