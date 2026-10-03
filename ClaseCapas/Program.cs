using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaseCapas
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Cualquier error que no se haya atrapado en un formulario se muestra
            // en un mensaje en vez de cerrar la aplicación (punto 7).
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => MostrarError(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => MostrarError(e.ExceptionObject as Exception);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }

        static void MostrarError(Exception ex)
        {
            string mensaje = ex != null ? ex.Message : "Error desconocido.";
            MessageBox.Show("Ocurrió un error inesperado:\n" + mensaje, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
