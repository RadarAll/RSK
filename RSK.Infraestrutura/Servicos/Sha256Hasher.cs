using RSK.Dominio.Autorizacao.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace RSK.Infraestrutura.Servicos
{
    public class Sha256Hasher : IHasher
    {
        public string Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Cria uma instância do SHA256
            using (var sha256 = SHA256.Create())
            {
                // Converte a string de entrada em um array de bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Calcula o hash
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Converte o hash (array de bytes) para uma representação hexadecimal (string)
                var builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    // "x2" formata o byte como um hexadecimal de 2 dígitos
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
