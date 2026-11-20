using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ClaseCapas.Controles
{
    // Control de usuario: Label + TextBox que se valida solo (obligatorio / solo números)
    public partial class CampoTexto : UserControl
    {
        public CampoTexto()
        {
            InitializeComponent();
        }

        [Category("Campo")]
        public string Titulo
        {
            get { return lblTitulo.Text; }
            set { lblTitulo.Text = value; }
        }

        [Category("Campo")]
        public string Texto
        {
            get { return txtValor.Text.Trim(); }
            set { txtValor.Text = value; }
        }

        [Category("Campo")]
        [DefaultValue(false)]
        public bool Requerido { get; set; }

        [Category("Campo")]
        [DefaultValue(false)]
        public bool SoloNumeros { get; set; }

        [Category("Campo")]
        [DefaultValue(false)]
        public bool SoloLectura
        {
            get { return txtValor.ReadOnly; }
            set { txtValor.ReadOnly = value; }
        }

        public event EventHandler TextoCambiado;

        // Marca el error con el ErrorProvider y devuelve si es válido
        public bool Validar()
        {
            string error = "";
            if (Requerido && Texto == "")
                error = Titulo + " es obligatorio.";
            else if (SoloNumeros && Texto != "" && !decimal.TryParse(Texto, out decimal n))
                error = Titulo + " tiene que ser un número.";

            errorProvider1.SetError(txtValor, error);
            return error == "";
        }

        public void Limpiar()
        {
            txtValor.Clear();
            errorProvider1.SetError(txtValor, "");
        }

        private void txtValor_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtValor, "");
            TextoCambiado?.Invoke(this, EventArgs.Empty);
        }
    }
}
