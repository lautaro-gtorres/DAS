using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Transferencia
    {
        private int idTransferencia;

        public int IdTransferencia
        {
            get { return idTransferencia; }
            set { idTransferencia = value; }
        }

        private int usuarioEmisor;

        public int UsuarioEmisor
        {
            get { return usuarioEmisor; }
            set { usuarioEmisor = value; }
        }

        private int usuarioReceptor;

        public int UsuarioReceptor
        {
            get { return usuarioReceptor; }
            set { usuarioReceptor = value; }
        }

        private double importe;

        public double Importe
        {
            get { return importe; }
            set { importe = value; }
        }

        private DateTime fechaHora;

        public DateTime FechaHora
        {
            get { return fechaHora; }
            set { fechaHora = value; }
        }

        // No es columna de la tabla transferencias: se guarda en los dos movimientos.
        private string concepto;

        public string Concepto
        {
            get { return concepto; }
            set { concepto = value; }
        }
    }
}
