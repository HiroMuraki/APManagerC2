namespace Encrypter.Interface {
    public interface IBytesEncrypter {
        /// <summary>
        /// 加密bytes
        /// </summary>
        /// <param name="buffer">要加密的bytes</param>
        /// <param name="size">加密大小</param>
        public void Encrypt(byte[] buffer, int size);
        /// <summary>
        /// 解密bytes
        /// </summary>
        /// <param name="buffer">要解密的bytes</param>
        /// <param name="size">解密大小</param>
        public void Decrypt(byte[] buffer, int size);
    }
}