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
    // Punto 1: ABM de pasajeros
    public partial class FrmPasajeros : Form
    {
        BLL.BLLPasajero pasajeroBLL = new BLL.BLLPasajero();
        int idSeleccionado = 0;

        public FrmPasajeros()
        {
            InitializeComponent();
        }

        void refresh()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = pasajeroBLL.Listar();
        }

        void Limpiar()
        {
            idSeleccionado = 0;
            txtNombre.Limpiar();
            txtApellido.Limpiar();
            txtDireccion.Limpiar();
        }

        bool DatosValidos()
        {
            // Se validan los 3 para que marque todos los errores
            bool a = txtNombre.Validar();
            bool b = txtApellido.Validar();
            bool c = txtDireccion.Validar();
            return a && b && c;
        }

        BE.Pasajero LeerFormulario()
        {
            BE.Pasajero p = new BE.Pasajero();
            p.IdPasajero = idSeleccionado;
            p.Nombre = txtNombre.Texto;
            p.Apellido = txtApellido.Texto;
            p.Direccion = txtDireccion.Texto;
            return p;
        }

        private void FrmPasajeros_Load(object sender, EventArgs e)
        {
            try
            {
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!DatosValidos()) return;
            try
            {
                int fa = pasajeroBLL.Alta(LeerFormulario());
                MessageBox.Show(fa != 0 ? "Pasajero agregado" : "No se pudo agregar");
                refresh();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!DatosValidos()) return;
            try
            {
                int fa = pasajeroBLL.Modificar(LeerFormulario());
                MessageBox.Show(fa != 0 ? "Pasajero modificado" : "No se pudo modificar");
                refresh();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            try
            {
                int fa = pasajeroBLL.Baja(idSeleccionado);
                MessageBox.Show(fa != 0 ? "Pasajero eliminado" : "No se pudo eliminar");
                refresh();
                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BE.Pasajero filtro = new BE.Pasajero();
                filtro.Apellido = txtBuscar.Text.Trim();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = pasajeroBLL.Buscar(filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            BE.Pasajero p = (BE.Pasajero)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            idSeleccionado = p.IdPasajero;
            txtNombre.Texto = p.Nombre;
            txtApellido.Texto = p.Apellido;
            txtDireccion.Texto = p.Direccion;
        }
    }
}
