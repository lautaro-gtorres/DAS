using MAPPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Comprobante de venta (no fiscal, tipo "X"): se arma con los datos de la venta.
    // Se puede ver, imprimir (o guardar como PDF con "Microsoft Print to PDF") y exportar a XML.
    public class BLLComprobante
    {
        // Datos de la granja que salen en el encabezado (ficticios).
        public const string Empresa = "LRCHICKENS - Granja avícola";
        public const string Cuit = "30-71234567-8";
        public const string Direccion = "Ruta 5 km 112, Mercedes, Buenos Aires";
        public const string Leyenda = "Comprobante X - No válido como factura";

        MapperVenta mapper = new MapperVenta();

        public BE.Comprobante Generar(int idVenta)
        {
            BE.Comprobante comprobante = mapper.ObtenerComprobante(idVenta);
            if (comprobante == null)
                throw new ArgumentException("No existe la venta " + idVenta + ".");
            return comprobante;
        }

        public void ExportarXML(int idVenta, string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                throw new ArgumentException("Elija dónde guardar el archivo.");
            Generar(idVenta);   // controla que la venta exista
            mapper.ExportarComprobanteXML(idVenta, ruta);
        }

        // El comprobante como texto, línea por línea (lo usan la pantalla y la impresión).
        public List<string> ArmarLineas(BE.Comprobante comprobante)
        {
            string separador = new string('-', 56);
            List<string> lineas = new List<string>();
            lineas.Add(Empresa);
            lineas.Add("CUIT: " + Cuit);
            lineas.Add(Direccion);
            lineas.Add(separador);
            lineas.Add("COMPROBANTE X   N° " + comprobante.NroComprobante);
            lineas.Add(Leyenda);
            lineas.Add("Fecha: " + comprobante.Fecha.ToString("dd/MM/yyyy"));
            lineas.Add("Cliente: " + comprobante.Cliente);
            lineas.Add(separador);
            lineas.Add(string.Format("{0,-6}{1,-24}{2,12}{3,14}", "Cant.", "Descripción", "P. unit.", "Subtotal"));
            // Si la descripción no entra en la columna, se corta en un espacio y sigue abajo.
            string primera = comprobante.Descripcion;
            string resto = "";
            if (primera.Length > 23)
            {
                int corte = primera.LastIndexOf(' ', 23);
                if (corte <= 0)
                    corte = 23;
                resto = primera.Substring(corte).Trim();
                primera = primera.Substring(0, corte);
            }
            lineas.Add(string.Format("{0,-6}{1,-24}{2,12}{3,14}", comprobante.Cantidad, primera,
                comprobante.PrecioUnitario.ToString("N2"), comprobante.Total.ToString("N2")));
            if (resto != "")
                lineas.Add("      " + resto);
            lineas.Add(separador);
            lineas.Add(string.Format("{0,-30}{1,26}", "TOTAL", "$ " + comprobante.Total.ToString("N2")));
            lineas.Add("");
            lineas.Add("¡Gracias por su compra!");
            return lineas;
        }
    }
}
