using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    /// <summary>
    /// User Control: caja de texto que valida su propio contenido mientras se escribe.
    /// - Según el TipoDato no deja tipear caracteres inválidos (por ejemplo letras en un número).
    /// - Al salir del campo muestra el error en rojo debajo.
    /// - EsValido() lo usa el formulario antes de guardar (y muestra el error con MessageBox).
    /// Solo controla el formato de lo que se escribe; las reglas del negocio siguen en la BLL.
    /// </summary>
    public partial class CampoValidado : UserControl
    {
        // Fondo normal (el mismo de los TextBox de los formularios) y fondo con error.
        Color colorNormal = Color.PeachPuff;
        Color colorError = Color.MistyRose;

        public CampoValidado()
        {
            InitializeComponent();
        }

        // ---- Propiedades (se configuran desde el diseñador) ----

        private TipoCampo tipoDato = TipoCampo.Texto;
        [Category("Validación"), Description("Qué tipo de dato acepta el campo.")]
        public TipoCampo TipoDato
        {
            get { return tipoDato; }
            set { tipoDato = value; }
        }

        private bool obligatorio = false;
        [Category("Validación"), Description("Si el campo no puede quedar vacío.")]
        public bool Obligatorio
        {
            get { return obligatorio; }
            set { obligatorio = value; }
        }

        private string nombreCampo = "El campo";
        [Category("Validación"), Description("Nombre que se usa en los mensajes de error (ej: \"El e-mail\").")]
        public string NombreCampo
        {
            get { return nombreCampo; }
            set { nombreCampo = value; }
        }

        [Category("Validación"), Description("Cantidad máxima de caracteres.")]
        public int LargoMaximo
        {
            get { return txtValor.MaxLength; }
            set { txtValor.MaxLength = value; }
        }

        // El texto escrito, sin espacios a los costados.
        [Browsable(false)]
        public string Valor
        {
            get { return txtValor.Text.Trim(); }
            set { txtValor.Text = value; }
        }

        // Mensaje del último error ("" si es válido).
        private string mensajeError = "";
        [Browsable(false)]
        public string MensajeError
        {
            get { return mensajeError; }
        }

        // Se dispara cada vez que cambia el texto (por ejemplo, para recalcular un total).
        public event EventHandler ValorCambiado;

        // ---- Validación ----

        // Devuelve true si el contenido es válido; si no, muestra el error debajo del campo.
        public bool EsValido()
        {
            mensajeError = BuscarError();
            lblError.Text = mensajeError;
            if (mensajeError == "")
                txtValor.BackColor = colorNormal;
            else
                txtValor.BackColor = colorError;
            return mensajeError == "";
        }

        // El error con el nombre del campo, para mostrarlo en un MessageBox
        // (ej: "La cantidad de huevos: solo números enteros.").
        public string ErrorCompleto()
        {
            if (mensajeError == "")
                return "";
            return nombreCampo + ": " + mensajeError.Substring(0, 1).ToLower() + mensajeError.Substring(1);
        }

        // Saca el error que se muestra (por ejemplo al limpiar el formulario).
        public void Limpiar()
        {
            txtValor.Text = "";
            mensajeError = "";
            lblError.Text = "";
            txtValor.BackColor = colorNormal;
        }

        // Pone el cursor en el campo.
        public void Enfocar()
        {
            txtValor.Focus();
        }

        string BuscarError()
        {
            string valor = Valor;
            if (valor == "")
            {
                if (obligatorio)
                    return "Es obligatorio.";
                return "";
            }

            switch (tipoDato)
            {
                case TipoCampo.Entero:
                    int entero;
                    if (!int.TryParse(valor, out entero))
                        return "Solo números enteros.";
                    if (entero < 0)
                        return "No puede ser negativo.";
                    break;
                case TipoCampo.Decimal:
                    decimal numero;
                    if (!decimal.TryParse(valor, out numero))
                        return "Tiene que ser un número.";
                    if (numero < 0)
                        return "No puede ser negativo.";
                    break;
                case TipoCampo.Email:
                    if (!BLL.Validaciones.EsEmailValido(valor))
                        return "E-mail no válido (ej: nombre@mail.com).";
                    break;
                case TipoCampo.Dni:
                    if (valor.Length < 7 || valor.Length > 8)
                        return "El DNI tiene 7 u 8 números.";
                    break;
                case TipoCampo.Telefono:
                    int digitos = 0;
                    foreach (char c in valor)
                    {
                        if (char.IsDigit(c))
                            digitos++;
                    }
                    if (digitos < 6)
                        return "Teléfono muy corto.";
                    break;
            }
            return "";
        }

        // ---- Eventos del TextBox ----

        // No deja escribir caracteres que no corresponden al tipo de dato.
        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            switch (tipoDato)
            {
                case TipoCampo.Entero:
                case TipoCampo.Dni:
                    if (!char.IsDigit(e.KeyChar))
                        e.Handled = true;
                    break;
                case TipoCampo.Decimal:
                    string separador = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    bool esSeparador = e.KeyChar.ToString() == separador;
                    if (!char.IsDigit(e.KeyChar) && !esSeparador)
                        e.Handled = true;
                    if (esSeparador && txtValor.Text.Contains(separador))
                        e.Handled = true;
                    break;
                case TipoCampo.Telefono:
                    if (!char.IsDigit(e.KeyChar) && "-+() ".IndexOf(e.KeyChar) < 0)
                        e.Handled = true;
                    break;
                case TipoCampo.Email:
                    if (e.KeyChar == ' ')
                        e.Handled = true;
                    break;
            }
        }

        // Al salir del campo se valida y se muestra el error, si hay.
        private void txtValor_Leave(object sender, EventArgs e)
        {
            EsValido();
        }

        // Mientras se escribe se saca el error anterior y se avisa al formulario.
        private void txtValor_TextChanged(object sender, EventArgs e)
        {
            if (mensajeError != "")
            {
                mensajeError = "";
                lblError.Text = "";
                txtValor.BackColor = colorNormal;
            }
            if (ValorCambiado != null)
                ValorCambiado(this, EventArgs.Empty);
        }
    }
}
