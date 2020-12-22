using System.Security.Cryptography;
using System.Text;

namespace Encrypter {
    public static class HashString {
        
        public static string SHA(string password) {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] result = SHA256.HashData(bytes);
            StringBuilder builder = new StringBuilder(16);

            foreach (byte item in result) {
                builder.Append($"{item:X2}");
            }

            return builder.ToString();
        }

        public static bool Compare(string a,string b) {
            return SHA(a) == SHA(b);
        }
    }
}
