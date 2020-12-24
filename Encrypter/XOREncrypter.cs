using Encrypter.Interface;
using System;
using System.Text.RegularExpressions;

namespace Encrypter {
    public class XOREncrypter : IBytesEncrypter {
        private string _key;
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

        public XOREncrypter(string key) {
            Key = key;
        }

        public void Decrypt(byte[] buffer, int size) {
            for (int i = 0; i < size; i++) {
                buffer[i] ^= GetToken(i);
            }
        }
        public void Encrypt(byte[] buffer, int size) {
            for (int i = 0; i < size; i++) {
                buffer[i] ^= GetToken(i);
            }
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