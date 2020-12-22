using Encrypter.Interface;
using System;
using System.IO;

namespace Encrypter {
    public class FileEncrypter {
        private const int BytePerMbyte = 1024;
        private const int _bufferSize = 32 * BytePerMbyte;

        public event EventHandler<EncryptProcessEventArgs> Processing;

        #region 属性
        /// <summary>
        /// 加密器
        /// </summary>
        private readonly IBytesEncrypter _encrypter;
        public IBytesEncrypter Encrypter {
            get {
                return _encrypter;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数，传入实现了IBytesEncrypter的加密器
        /// </summary>
        /// <param name="encrypter"></param>
        public FileEncrypter(IBytesEncrypter encrypter) {
            _encrypter = encrypter;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="sourceFile">源文件路径</param>
        /// <param name="outputFile">加密的文件路径</param>
        public void Encrypt(string sourceFile, string outputFile) {
            EncryptCore(sourceFile, outputFile, Encrypter.Encrypt);
        }
        public void Encrypt(string filePath) {
            string tempFile = GetTempFileName(filePath);
            Encrypt(filePath, tempFile);
            File.Move(tempFile, filePath, true);
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="sourceFile">源文件路径</param>
        /// <param name="outputFile">解密的文件路径</param>
        public void Decrypt(string sourceFile, string outputFile) {
            EncryptCore(sourceFile, outputFile, Encrypter.Decrypt);
        }
        public void Decrypt(string filePath) {
            string tempFile = GetTempFileName(filePath);
            Decrypt(filePath, tempFile);
            File.Move(tempFile, filePath, true);
        }
        #region 辅助方法
        private static string GetTempFileName(string sourceFilePath) {
            string filename = Path.GetFileNameWithoutExtension(sourceFilePath);
            return $"__{filename}__.tef";
        }
        private void EncryptCore(string sourceFile, string outputFile, Action<byte[], int> action) {
            if (File.Exists(outputFile)) {
                throw new IOException($"文件{outputFile}被占用");
            }

            byte[] buffer = new byte[_bufferSize];
            double totalRead = 0;

            using (FileStream source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read)) {
                using (FileStream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write)) {
                    long fileSize = source.Length;
                    while (true) {
                        int readCount = source.Read(buffer, 0, _bufferSize);
                        if (readCount == 0) {
                            break;
                        }
                        totalRead += readCount;

                        action(buffer, readCount);
                        Processing?.Invoke(this, new EncryptProcessEventArgs(totalRead / fileSize));

                        output.Write(buffer);
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}