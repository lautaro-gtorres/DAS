using System;
using System.Security.Cryptography;

namespace BLL
{
    /// <summary>
    /// Hash de contraseñas con PBKDF2 (SHA-256 + sal aleatoria).
    /// Formato guardado en la base: PBKDF2$iteraciones$salBase64$hashBase64
    /// </summary>
    public static class Encriptador
    {
        private const int Iteraciones = 10000;
        private const int BytesSal = 16;
        private const int BytesHash = 32;

        public static string Hashear(string contrasena)
        {
            if (contrasena == null)
                throw new ArgumentNullException("contrasena");

            byte[] sal = new byte[BytesSal];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(sal);
            }
            byte[] hash = Derivar(contrasena, sal, Iteraciones, BytesHash);
            return "PBKDF2$" + Iteraciones + "$" + Convert.ToBase64String(sal) + "$" + Convert.ToBase64String(hash);
        }

        public static bool Verificar(string contrasena, string hashGuardado)
        {
            if (contrasena == null || string.IsNullOrEmpty(hashGuardado))
                return false;

            string[] partes = hashGuardado.Split('$');
            int iteraciones;
            if (partes.Length != 4 || partes[0] != "PBKDF2" || !int.TryParse(partes[1], out iteraciones))
                return false;

            try
            {
                byte[] sal = Convert.FromBase64String(partes[2]);
                byte[] esperado = Convert.FromBase64String(partes[3]);
                byte[] calculado = Derivar(contrasena, sal, iteraciones, esperado.Length);
                return SonIguales(calculado, esperado);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] Derivar(string contrasena, byte[] sal, int iteraciones, int largo)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(contrasena, sal, iteraciones, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(largo);
            }
        }

        // Compara todos los bytes siempre, para no dar pistas por el tiempo de respuesta.
        private static bool SonIguales(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int diferencia = 0;
            for (int i = 0; i < a.Length; i++)
                diferencia |= a[i] ^ b[i];
            return diferencia == 0;
        }
    }
}
