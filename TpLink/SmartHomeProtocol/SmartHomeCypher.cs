﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    public static class SmartHomeCypher
    {
        /// <summary>
        /// Initial cypher key value to use when encrypting and decrypting data transmissions.
        /// </summary>
        private const int INITIAL_CYPHERKEY = 171;

        /// <summary>
        /// Encrypts data before being transmitted to the smart home device
        /// </summary>
        /// <param name="data">Data to be encrypted</param>
        /// <returns>Encrypted byte array</returns>
        public static byte[] Encrypt(string data)
        {
            var result = new List<byte>() { 0, 0, 0, (byte)data.Length };

            var key = INITIAL_CYPHERKEY;

            foreach (int dataChar in data)
            {
                var a = key ^ dataChar;
                key = a;
                result.Add((byte)a);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Decrypts data sent back from the smart home device
        /// </summary>
        /// <param name="data">Encrypted byte array</param>
        /// <param name="bufferLength">Length of buffer array</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(byte[] data, int bufferLength)
        {
            var key = INITIAL_CYPHERKEY;

            var result = new StringBuilder();
            for (int i = 4; i < bufferLength; i++)
            {
                if (data[i] == 0)
                {
                    break;
                }

                var a = key ^ data[i];
                key = data[i];

                result.Append((char)a);
            }
            return result.ToString();
        }
    }
}