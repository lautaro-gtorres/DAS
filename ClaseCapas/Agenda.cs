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
    /// Agenda: próximas vacunaciones (de hoy en adelante), la más cercana primero.
    /// Actualizar abre la vacunación para modificarla; Eliminar la borra.
    /// </summary>
    public partial class Agenda : Form
    {
        BLL.BLLVacunacion bll = new BLL.BLLVacunacion();

        // Para volver al menú desde el que se abrió (Inicio o Ventas).
        bool desdeInicio = false;

        public Agenda()
        {
            InitializeComponent();
        }

        public Agenda(bool desdeInicio)
        {
            InitializeComponent();
            this.desdeInicio = desdeInicio;
        }

        private void Agenda_Load(object sender, EventArgs e)
        {
            Pantalla.PrepararGrilla(dgvAgenda);
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            try
            {
                Pantalla.Llenar(dgvAgenda, bll.ListarProximas());
                Pantalla.Titulo(dgvAgenda, "Nombre", "Vacuna");
                Pantalla.Titulo(dgvAgenda, "NroCorral", "Corral");
                Pantalla.Titulo(dgvAgenda, "FechaAplicacion", "Última aplicación");
                Pantalla.Titulo(dgvAgenda, "ProximaAplicacion", "Próxima aplicación");
                dgvAgenda.Columns["FechaAplicacion"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvAgenda.Columns["ProximaAplicacion"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Agenda");
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BE.Vacunacion vacunacion = (BE.Vacunacion)Pantalla.Seleccionado(dgvAgenda);
            if (vacunacion == null)
            {
                Pantalla.Avisar("Seleccione una vacunación de la lista.", "Agenda");
                return;
            }
            CargarVacunas editar = new CargarVacunas(vacunacion);
            if (editar.ShowDialog(this) == DialogResult.OK)
                CargarGrilla();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            BE.Vacunacion vacunacion = (BE.Vacunacion)Pantalla.Seleccionado(dgvAgenda);
            if (vacunacion == null)
            {
                Pantalla.Avisar("Seleccione una vacunación de la lista.", "Agenda");
                return;
            }
            if (!Pantalla.Confirmar("¿Eliminar la vacunación " + vacunacion.Nombre + " del corral " + vacunacion.NroCorral + "?", "Agenda"))
                return;
            try
            {
                bll.Baja(vacunacion.IdVacunacion);
                CargarGrilla();
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Agenda");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (desdeInicio)
            {
                Inicio inicio = new Inicio();
                inicio.Show();
            }
            else
            {
                Ventas ventas = new Ventas();
                ventas.Show();
            }
            this.Close();
        }
    }
}
