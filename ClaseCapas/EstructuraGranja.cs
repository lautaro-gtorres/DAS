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
    /// Muestra la granja como árbol (patrón Composite): Granja → Ubicación → Corral.
    /// Al elegir cualquier nodo se muestran sus gallinas, capacidad, lugares libres y huevos
    /// del último mes: el formulario llama a los mismos métodos sin importar si es un corral o un grupo.
    /// </summary>
    public partial class EstructuraGranja : Form
    {
        BLL.BLLGranja bll = new BLL.BLLGranja();

        public EstructuraGranja()
        {
            InitializeComponent();
        }

        private void EstructuraGranja_Load(object sender, EventArgs e)
        {
            try
            {
                BE.ComponenteGranja granja = bll.ArmarEstructura();
                tvGranja.Nodes.Clear();
                AgregarNodo(tvGranja.Nodes, granja);
                tvGranja.ExpandAll();
                tvGranja.SelectedNode = tvGranja.Nodes[0];
            }
            catch (Exception ex)
            {
                Pantalla.MostrarError(ex, "Estructura de la granja");
            }
        }

        // Agrega el componente y, de forma recursiva, todos sus hijos.
        private void AgregarNodo(TreeNodeCollection nodos, BE.ComponenteGranja componente)
        {
            TreeNode nodo = nodos.Add(componente.Nombre + "  (" + componente.Gallinas() + " gallinas)");
            nodo.Tag = componente;
            foreach (BE.ComponenteGranja hijo in componente.Hijos())
                AgregarNodo(nodo.Nodes, hijo);
        }

        // Muestra los datos del nodo elegido. Si es un grupo, cada valor ya es la suma de sus hijos.
        private void tvGranja_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BE.ComponenteGranja componente = (BE.ComponenteGranja)e.Node.Tag;
            lblNombre.Text = componente.Nombre;

            string capacidad = componente.Capacidad().ToString();
            string libres = componente.LugaresLibres().ToString();
            string ocupacion = componente.Ocupacion() + " %";
            if (componente.Capacidad() == 0)
            {
                capacidad = "sin cargar";
                libres = "-";
                ocupacion = "-";
            }

            lblDetalle.Text = "Tipo: " + componente.Tipo() + "\n\n" +
                              "Gallinas: " + componente.Gallinas() + "\n" +
                              "Capacidad: " + capacidad + "\n" +
                              "Lugares libres: " + libres + "\n" +
                              "Ocupación: " + ocupacion + "\n\n" +
                              "Huevos del último mes: " + componente.HuevosUltimoMes();
            if (componente.Hijos().Count > 0)
                lblDetalle.Text = lblDetalle.Text + "\n\nContiene: " + componente.Hijos().Count + " elementos";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
