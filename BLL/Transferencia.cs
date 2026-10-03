using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Transferencia
    {
        DAL.Transferencia dal = new DAL.Transferencia();
        Usuario usuarioBLL = new Usuario();

        // Valida todo lo que pide el punto 3 del enunciado y devuelve el destinatario.
        // La pantalla la usa antes de pedir la confirmación.
        public BE.Usuario Validar(int idEmisor, string usuarioDestinatario, double importe)
        {
            BE.Usuario emisor = usuarioBLL.ObtenerPorId(idEmisor);
            if (emisor == null || !emisor.Estado)
                throw new ArgumentException("Tu usuario no está habilitado para operar.");

            if (string.IsNullOrWhiteSpace(usuarioDestinatario))
                throw new ArgumentException("Ingresá el usuario destinatario.");

            BE.Usuario receptor = usuarioBLL.ObtenerPorNombreUsuario(usuarioDestinatario.Trim());
            if (receptor == null)
                throw new ArgumentException("El usuario destinatario no existe.");
            if (!receptor.Estado)
                throw new ArgumentException("El usuario destinatario está inactivo.");
            if (receptor.IdUsuario == emisor.IdUsuario)
                throw new ArgumentException("No podés transferirte a vos mismo.");
            if (receptor.Tipo != Usuario.TipoCliente)
                throw new ArgumentException("Solo se puede transferir a otros clientes.");

            if (importe <= 0)
                throw new ArgumentException("El importe debe ser mayor que cero.");
            if (emisor.Saldo < importe)
                throw new ArgumentException("Saldo insuficiente.");

            return receptor;
        }

        // Devuelve el id de la transferencia realizada.
        public int Realizar(int idEmisor, string usuarioDestinatario, double importe, string concepto)
        {
            importe = Math.Round(importe, 2);
            // Se vuelve a validar justo antes de operar (el saldo pudo cambiar).
            BE.Usuario receptor = Validar(idEmisor, usuarioDestinatario, importe);

            BE.Transferencia transferencia = new BE.Transferencia();
            transferencia.UsuarioEmisor = idEmisor;
            transferencia.UsuarioReceptor = receptor.IdUsuario;
            transferencia.Importe = importe;
            transferencia.Concepto = string.IsNullOrWhiteSpace(concepto) ? "Transferencia" : concepto.Trim();

            return dal.Realizar(transferencia);
        }
    }
}
