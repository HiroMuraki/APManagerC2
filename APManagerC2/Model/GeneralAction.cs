using System.Security.Cryptography;
using System.Text;

namespace APManager2.Test {
    public static class GeneralAction {
        public const string UserProfileFolderName = "UserProfile";
        public const string UserStorageFileName = "Storage.db";
        public const string UserDataFileName = "UserData";
        public const string UserAvatarFileName = "Avatar";
        public const string DataAvatarsFolderName = "DataAvatars";

        /// <summary>
        /// 根据SHA3算法获取字符串的散列值
        /// </summary>
        /// <param name="sourceString">原字符串</param>
        /// <returns></returns>
        public static string SHAString(string sourceString) {
            byte[] byteArray = Encoding.UTF8.GetBytes(sourceString);
            byte[] shaValue = SHA256.Create().ComputeHash(byteArray);

            StringBuilder sb = new StringBuilder();
            foreach (byte code in shaValue) {
                sb.Append($"{code:X2}");
            }

            return sb.ToString();
        }
    }
}
