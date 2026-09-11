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

       

        

        private void Form1_Load(object sender, EventArgs e)
        {
           
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
            AbrirHijo(new FrmBuscarPersona());
        }

        private void resumenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmResumen());
        }

       
    }
}
