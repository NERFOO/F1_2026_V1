//using System.Security.Cryptography;
//using System.Text;

//namespace F1.Helpers
//{
//    public class HelperEncrypt
//    {
//        public static string GenerateSalt()
//        {
//            //TENDREMOS UN SALT DE 24
//            Random random = new Random();
//            string salt = "";
//            for (int i = 1; i <= 24; i++)
//            {
//                int aleat = random.Next(0, 255);
//                char letra = Convert.ToChar(aleat);
//                salt += letra;
//            }

//            return salt;
//        }

//        //EN ALGUN MOMENTO TENDREMOS QUE COMPARAR SI LOS PASSWORD SON IGUALES
//        public static bool CompareArray(byte[] a, byte[] b)
//        {
//            bool iguales = true;

//            if (a.Length != b.Length)
//            {
//                iguales = false;
//            }
//            else
//            {
//                for (int i = 0; i < a.Length; i++)
//                {
//                    if (a[i].Equals(b[i]) == false)
//                    {
//                        iguales = false;
//                        break;
//                    }
//                }
//            }
//            return iguales;
//        }


//        //TENDREMOS UN METODO PARA CIFRAR NUESTRA PASSWORD
//        public static byte[] EncryptPassword(string password, string salt)
//        {
//            string contenido = password + salt;
//            SHA512 sHA = SHA512.Create();

//            //CONVERTIMOS NUESTRO CONTENIDO A BYTES[]
//            byte[] salida = Encoding.UTF8.GetBytes(contenido);

//            //LAS ITERACCIONES PARA NUESTRO PASSWORD
//            for (int i = 1; i <= 24; i++)
//            {
//                salida = sHA.ComputeHash(salida);
//            }

//            sHA.Clear();

//            return salida;
//        }
//    }
//}
using System.Security.Cryptography;

namespace F1.Helpers
{
	public static class HelperEncrypt
	{
		private const int SaltSize = 16;          // 128 bits
		private const int HashSize = 64;          // 512 bits
		private const int Iterations = 100_000;   // coste real

		public static byte[] GenerateSalt()
		{
			byte[] salt = new byte[SaltSize];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
			return salt;
		}

		public static byte[] HashPassword(string password, byte[] salt)
		{
			using var pbkdf2 = new Rfc2898DeriveBytes(
				password,
				salt,
				Iterations,
				HashAlgorithmName.SHA512
			);

			return pbkdf2.GetBytes(HashSize);
		}

		public static bool VerifyPassword(
			string password,
			byte[] storedHash,
			byte[] storedSalt)
		{
			var hashToCompare = HashPassword(password, storedSalt);
			return CryptographicOperations.FixedTimeEquals(storedHash, hashToCompare);
		}
	}
}

