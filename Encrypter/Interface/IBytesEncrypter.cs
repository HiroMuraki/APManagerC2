﻿namespace Encrypter.Interface {
    public interface IBytesEncrypter {
        /// <summary>
        /// 加密缓冲区大小
        /// </summary>
        int EncryptBufferSize { get; }
        /// <summary>
        /// 解密缓冲区大小
        /// </summary>
        int DecryptBufferSize { get; }
        /// <summary>
        /// 加密字节流
        /// </summary>
        /// <param name="buffer">要加密的字节流</param>
        /// <returns>加密后的字节流</returns>
        byte[] Encrypt(byte[] byteArray);
        /// <summary>
        /// 解密字节流
        /// </summary>
        /// <param name="buffer">要加密的字节流</param>
        /// <returns>解密后的字节流</returns>
        byte[] Decrypt(byte[] byteArray);
    }
}