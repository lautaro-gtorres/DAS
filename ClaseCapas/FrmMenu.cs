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
    // Formulario MDI principal
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        // Si el formulario ya está abierto lo activa, si no lo abre como hijo
        void AbrirHijo(Form hijo)
        {
            foreach (Form abierto in this.MdiChildren)
            {
                if (abierto.GetType() == hijo.GetType())
                {
                    abierto.Activate();
                    return;
                }
            }

            hijo.MdiParent = this;
            hijo.Show();
        }

        private void pasajerosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmPasajeros());
        }

        private void viajesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmViajes());
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmReportes());
        }
    }
}
