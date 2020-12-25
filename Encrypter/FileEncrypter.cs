using Encrypter.Interface;
using System;
using System.IO;

namespace Encrypter {
    public class FileEncrypter {
        private IBytesEncrypter _encrypter;

        public FileEncrypter(IBytesEncrypter encrypter) {
            _encrypter = encrypter;
        }

        public void Encrypt(string sourceFile, string outputFile) {
            ProcessCore(sourceFile, outputFile, _encrypter.Encrypt, _encrypter.EncryptBufferSize);

        }
        public void Encrypt(string sourceFile) {
            string tempFileName = GetTempFileName(sourceFile);
            Encrypt(sourceFile, tempFileName);
            File.Move(tempFileName, sourceFile, true);
        }
        public void Decrypt(string sourceFile, string outputFile) {
            ProcessCore(sourceFile, outputFile, _encrypter.Decrypt, _encrypter.DecryptBufferSize);
        }
        public void Decrypt(string sourceFile) {
            string tempFileName = GetTempFileName(sourceFile);
            Decrypt(sourceFile, tempFileName);
            File.Move(tempFileName, sourceFile, true);
        }

        private string GetTempFileName(string filePath) {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            return $"__{fileName}__.temp";
        }
        private void ProcessCore(string sourceFile, string outputFile, Func<byte[], byte[]> processor, int bufferSize) {
            byte[] buffer = new byte[bufferSize];

            using (FileStream source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read)) {
                using (FileStream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write)) {
                    while (true) {
                        int readCount = source.Read(buffer, 0, bufferSize);
                        if (readCount == 0) {
                            break;
                        }
                        byte[] processedFile = processor(buffer);
                        output.Write(processedFile, 0, processedFile.Length);
                    }
                }
            }
        }
    }
}