using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encrypter.Interface;
using System.Security.Cryptography;
using System.IO;
using static Encrypter.Common;

namespace Encrypter {
    public class AESEncrypter : IBytesEncrypter {
        const int _aesKeyLength = 16; // AES密钥字节长度，默认为128位
        const int _aesBlockSize = _aesKeyLength * BitPerByte; //AES加密块大小，设置为密钥长度*BitPerByte
        
        private Aes _aes;
        private ICryptoTransform _encryter; // 加密转置
        private ICryptoTransform _decrypter; // 解密转置
        private readonly int _encryptBufferSize = 32 * BytePerKByte; // 加密缓冲区大小，设置为32KB
        private readonly int _decryptBufferSize = 32 * BytePerKByte + 16; // 解密缓冲区大小，为加密缓冲区大小加上AES加密会导致多出的16字节

        public int EncryptBufferSize {
            get {
                return _encryptBufferSize;
            }
        }
        public int DecryptBufferSize {
            get {
                return _decryptBufferSize;
            }
        }

        public AESEncrypter(string key) {
            _aes = Aes.Create();
            _aes.BlockSize = _aesBlockSize;
            _aes.Key = GetKey(key);
            _aes.IV = new byte[_aesKeyLength];
            for (int i = 0; i < _aesKeyLength; i++) {
                _aes.IV[i] = _aes.Key[i];
            }
            _encryter = _aes.CreateEncryptor();
            _decrypter = _aes.CreateDecryptor();
        }
        public AESEncrypter() : this("") {

        }

        /// <summary>
        /// 加密字节流
        /// </summary>
        /// <param name="buffer">要加密的字节流</param>
        /// <returns>加密后的字节流</returns>
        public byte[] Decrypt(byte[] buffer) {
            return ProcessCore(buffer, _decrypter);
        }
        /// <summary>
        /// 解密字节流
        /// </summary>
        /// <param name="buffer">要加密的字节流</param>
        /// <returns>解密后的字节流</returns>
        public byte[] Encrypt(byte[] buffer) {
            return ProcessCore(buffer, _encryter);
        }

        /// <summary>
        /// 从字符串获取加密密钥
        /// </summary>
        /// <param name="key">字符串</param>
        /// <returns>长度为16（128位）的加密密钥</returns>
        private static byte[] GetKey(string key) {
            byte[] keys = new byte[_aesKeyLength];
            byte[] sourceKeys = Encoding.UTF8.GetBytes(key);
            //从字符串中获取密钥，若字符串长度大于16，则取前16位
            //若不足16位，则用0填充末尾
            for (int i = 0; i < _aesKeyLength; i++) {
                if (i < sourceKeys.Length) {
                    keys[i] = sourceKeys[i];
                } else {
                    keys[i] = 0;
                }
            }
            return keys;
        }
        /// <summary>
        /// 处理加/解密主要方法
        /// </summary>
        /// <param name="buffer">要加/解密的流</param>
        /// <param name="transform">转换阵</param>
        /// <returns>处理后的流</returns>
        private byte[] ProcessCore(byte[] buffer, ICryptoTransform transform) {
            byte[] output;

            using (MemoryStream memoryStream = new MemoryStream()) {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write)) {
                    using (BufferedStream writer = new BufferedStream(cryptoStream)) {
                        writer.Write(buffer);
                    }
                    output = memoryStream.ToArray();
                }
            }

            return output;
        }
    }
}
