using APMCore.Model;
using System.IO;
using System.Runtime.Serialization.Json;

namespace APMCore.Helper {
    internal static class UserDataHelper {
        /// <summary>
        /// 
        /// </summary>
        private readonly static DataContractJsonSerializer SourceSerializer = new DataContractJsonSerializer(typeof(Model.UserData));
        /// <summary>
        /// 从文件中读取UserData
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static UserData LoadFromFile(string filePath) {
            UserData userData = null;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                userData = SourceSerializer.ReadObject(file) as UserData;
            }
            return userData;
        }
        /// <summary>
        /// 将UserData保存至指定文件
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="filePath"></param>
        public static void SaveToFile(UserData userData, string filePath) {
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
                SourceSerializer.WriteObject(file, userData);
            }
        }
    }
}
