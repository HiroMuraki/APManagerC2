using Encrypter.Interface;
using System;
using System.Text.RegularExpressions;

namespace Encrypter {
    public class XOREncrypter : IBytesEncrypter {
        private string _key;
        private readonly int _encryptBufferSize = 32 * 1024;
        private readonly int _decryptBufferSize = 32 * 1024;

        /// <summary>
        /// 密钥
        /// </summary>
        public string Key {
            get {
                return _key;
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    throw new InvalidOperationException("密码不可为空");
                }
                if (!Regex.IsMatch(value, @"^[\u0020-\u007e]+$")) {
                    throw new InvalidOperationException("密码必须由ASCII可打印字符构成");
                }
                _key = value;
            }
        }
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

        public XOREncrypter(string key) {
            Key = key;
        }

        public byte[] Decrypt(byte[] buffer) {
            int size = buffer.Length;
            byte[] output = new byte[size];
            for (int i = 0; i < size; i++) {
                output[i] = (byte)(buffer[i] ^ GetToken(i));
            }
            return output;
        }
        public byte[] Encrypt(byte[] buffer) {
            int size = buffer.Length;
            byte[] output = new byte[size];
            for (int i = 0; i < size; i++) {
                output[i] = (byte)(buffer[i] ^ GetToken(i));
            }
            return output;
        }

        /// <summary>
        /// 获取加密token
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private byte GetToken(int pos) {
            if (pos % 2 == 0) {
                pos <<= 5;
            }
            if (pos % 3 == 0) {
                pos ^= pos * Key.Length;
            }
            return (byte)Key[pos % Key.Length];
        }
    }
}