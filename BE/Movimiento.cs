using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Movimiento
    {
        private int idMovimiento;

        public int IdMovimiento
        {
            get { return idMovimiento; }
            set { idMovimiento = value; }
        }

        private DateTime fechaHora;

        public DateTime FechaHora
        {
            get { return fechaHora; }
            set { fechaHora = value; }
        }

        // "Egreso" (transferencia enviada) o "Ingreso" (transferencia recibida)
        private string tipo;

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private double importe;

        public double Importe
        {
            get { return importe; }
            set { importe = value; }
        }

        // Nombre de usuario de la contraparte (para mostrar en el estado de cuenta)
        private string usuarioRelacionado;

        public string UsuarioRelacionado
        {
            get { return usuarioRelacionado; }
            set { usuarioRelacionado = value; }
        }

        private string concepto;

        public string Concepto
        {
            get { return concepto; }
            set { concepto = value; }
        }

        private int idUsuario;

        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        private int idUsuarioRelacionado;

        public int IdUsuarioRelacionado
        {
            get { return idUsuarioRelacionado; }
            set { idUsuarioRelacionado = value; }
        }

        private int idTransferencia;

        public int IdTransferencia
        {
            get { return idTransferencia; }
            set { idTransferencia = value; }
        }
    }
}
