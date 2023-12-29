using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.SmartHome.TpLink.SmartHomeProtocol
{
    /// <summary>
    /// Used to encrypt and decrypt data sent to and from the TP-Link smart home device.
    /// </summary>
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
        public static byte[] Encrypt(string data, bool usePadding)
        {

            List<byte> result = usePadding ? new() { 0, 0, 0, (byte)data.Length } : new();
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
        public static string Decrypt(byte[] data, bool usePadding)
        {
            var key = INITIAL_CYPHERKEY;

            var result = new StringBuilder();
            for (int i = (usePadding ? 4 : 0); i < data.Length; i++)
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
