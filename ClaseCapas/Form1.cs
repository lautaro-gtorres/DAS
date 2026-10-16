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
            

        }

        void refresh()
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            
        }

        
        public void LimpiarCampos()
        {
            
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}
