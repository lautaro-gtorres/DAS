using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    /// <summary>
    /// Cosas que usan todos los formularios: mensajes y grillas.
    /// </summary>
    public static class Pantalla
    {
        // Los errores de validación (ArgumentException) se muestran como aviso;
        // cualquier otro, como error de conexión o de la base.
        public static void MostrarError(Exception ex, string titulo)
        {
            if (ex is ArgumentException)
                MessageBox.Show(ex.Message, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Ocurrió un error al acceder a la base de datos.\n\n" + ex.Message, titulo,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Avisar(string mensaje, string titulo)
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool Confirmar(string pregunta, string titulo)
        {
            return MessageBox.Show(pregunta, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        // Grilla de solo lectura, se elige una fila entera por vez.
        public static void PrepararGrilla(DataGridView grilla)
        {
            grilla.ReadOnly = true;
            grilla.AllowUserToAddRows = false;
            grilla.AllowUserToDeleteRows = false;
            grilla.MultiSelect = false;
            grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grilla.RowHeadersVisible = false;
        }

        // Carga la lista en la grilla y oculta las columnas de Id.
        public static void Llenar(DataGridView grilla, object lista)
        {
            grilla.DataSource = null;
            grilla.DataSource = lista;
            foreach (DataGridViewColumn columna in grilla.Columns)
            {
                if (columna.Name.StartsWith("Id"))
                    columna.Visible = false;
            }
            grilla.ClearSelection();
        }

        // Cambia el título de una columna (si existe).
        public static void Titulo(DataGridView grilla, string columna, string titulo)
        {
            if (grilla.Columns.Contains(columna))
                grilla.Columns[columna].HeaderText = titulo;
        }

        public static void Ocultar(DataGridView grilla, string columna)
        {
            if (grilla.Columns.Contains(columna))
                grilla.Columns[columna].Visible = false;
        }

        // El objeto de la fila elegida, o null si no hay ninguna.
        public static object Seleccionado(DataGridView grilla)
        {
            if (grilla.CurrentRow == null || grilla.SelectedRows.Count == 0)
                return null;
            return grilla.CurrentRow.DataBoundItem;
        }
    }
}
