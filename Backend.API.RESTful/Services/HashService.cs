using System.Security.Cryptography;
using System.Text;

namespace Backend.API.RESTful.Services
{
    public class HashService
    {
        public string GenerateSHA256(string message)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(message));

                StringBuilder sb = new StringBuilder();

                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
