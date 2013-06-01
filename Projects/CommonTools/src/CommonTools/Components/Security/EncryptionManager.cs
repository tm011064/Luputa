using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.ComponentModel;

namespace CommonTools.Components.Security
{
    /// <summary>
    /// Provides methods for encrypting and decrypting data. This is a
    /// replacement for the old Cipher class available in utilities.
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    public static class EncryptionManager
    {
        /// <summary>
        /// Generates a hash for the given plain text value and returns a
        /// base64-encoded result. Before the hash is computed, a random salt
        /// is generated and appended to the plain text. This salt is stored at
        /// the end of the hash value, so it can be used later for hash
        /// verification.
        /// </summary>
        /// <param name="pText">
        /// Plaintext value to be hashed. The function does not check whether
        /// this parameter is null.
        /// </param>
        /// <param name="pHashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt
        /// value will be generated.
        /// </param>
        /// <returns>
        /// Hash value formatted as a base64-encoded string.
        /// </returns>
        public static string ComputeHash(string pText,
                                         SimpleHashAlgorithm pHashAlgorithm,
                                         byte[] saltBytes)
        {
            return SimpleHash.ComputeHash(pText, pHashAlgorithm, saltBytes);
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash
        /// value. Plain text is hashed with the same salt value as the original
        /// hash.
        /// </summary>
        /// <param name="pText">
        /// Plain text to be verified against the specified hash. The function
        /// does not check whether this parameter is null.
        /// </param>
        /// <param name="pHashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="pCipherText">
        /// Base64-encoded hash value produced by ComputeHash function. This value
        /// includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash mathes the specified hash the function the return
        /// value is true; otherwise, the function returns false.
        /// </returns>
        public static bool VerifyHash(string pText,
                                      SimpleHashAlgorithm pHashAlgorithm,
                                      string pCipherText)
        {
            return SimpleHash.VerifyHash(pText, pHashAlgorithm, pCipherText);
        }

        /// <summary>
        /// Advanced. Used to encrypt a string into a crypt32.dll:CryptProtectData encoded.
        /// </summary>
        /// <param name="textToEncrypt">The text to encrypt.</param>
        /// <returns></returns>
        public static byte[] EncryptDPAPIToBytes(string textToEncrypt)
        {
            try
            {
                DataProtector dp = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);
                byte[] dataToEncrypt = Encoding.ASCII.GetBytes(textToEncrypt);
                return dp.Encrypt(dataToEncrypt, null);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Advanced and overloaded. Used to decrypt a string with crypt32.dll:CryptUnprotectData.
        /// </summary>
        /// <param name="textToDecrypt"></param>
        /// <returns></returns>
        public static byte[] DecryptDPAPIToBytes(string textToDecrypt)
        {
            try
            {
                DataProtector dp = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);

                byte[] dataToDecrypt = Convert.FromBase64String(textToDecrypt);
                return dp.Decrypt(dataToDecrypt, null);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Advanced and overloaded. Used to decrypt a block of bytes with crypt32.dll:CryptUnprotectData.
        /// and then return the ascii text of that string.
        /// </summary>
        /// <param name="textToDecrypt"></param>
        /// <returns></returns>
        public static string DecryptDPAPI(byte[] textToDecrypt)
        {
            try
            {
                DataProtector dp = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);
                return Encoding.ASCII.GetString(dp.Decrypt(textToDecrypt, null));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Advanced and overloaded. Used to decrypt a string with crypt32.dll:CryptUnprotectData.
        /// and then return the ascii text of that string.
        /// </summary>
        /// <param name="textToDecrypt"></param>
        /// <returns></returns>
        public static string DecryptDPAPI(string textToDecrypt)
        {
            try
            {
                DataProtector dp = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);

                byte[] dataToDecrypt = Convert.FromBase64String(textToDecrypt);
                return Encoding.ASCII.GetString(dp.Decrypt(dataToDecrypt, null));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Overloaded. Used to encrypt a string with
        /// crypt32.dll:CryptProtectData and return the base64 representation of the ciphertext.
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <returns></returns>
        public static string EncryptDPAPI(string textToEncrypt)
        {
            try
            {
                DataProtector dp = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);
                byte[] dataToEncrypt = Encoding.ASCII.GetBytes(textToEncrypt);
                return Convert.ToBase64String(dp.Encrypt(dataToEncrypt, null));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTextToEncrypt"></param>
        /// <param name="pPassPhrase"></param>
        /// <param name="pInitVector"></param>
        /// <param name="pSaltValue"></param>
        /// <returns></returns>
        public static string AES_Simple_Encrypt(string pTextToEncrypt, string pPassPhrase, string pInitVector, string pSaltValue)
        {
            return RijndaelSimple.Encrypt(pTextToEncrypt, pPassPhrase, pSaltValue, pInitVector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTextToDecrypt"></param>
        /// <param name="pPassPhrase"></param>
        /// <param name="pInitVector"></param>
        /// <param name="pSaltValue"></param>
        /// <returns></returns>
        public static string AES_Simple_Decrypt(string pTextToDecrypt, string pPassPhrase, string pInitVector, string pSaltValue)
        {
            return RijndaelSimple.Decrypt(pTextToDecrypt, pPassPhrase, pSaltValue, pInitVector);
        }

        /// <summary>
        /// Encrypts a string using the Rijndael algo and encodes it into base64 format.
        /// Plaintext, salt, passphrase and initialization vectors are provided.
        /// </summary>
        /// <param name="text">The text to encrypt</param>
        /// <param name="passPhrase">The pass phrase used for this encryption</param>
        /// <param name="saltValue">The salt value used for this encryption</param>
        /// <param name="initVector">The init vector used for this encryption (must be 16 bit)</param>
        /// <param name="hashAlgorithm">The hash algorithm used for this encryption</param>
        /// <param name="keySize">The size of the key used for this encryption.</param>
        /// <param name="passWordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.</param>
        /// <returns></returns>
        public static string AES_Simple_Encrypt(string text, string passPhrase, string saltValue
            , string initVector, RijndaelSimpleHashAlgorithm hashAlgorithm, RijndaelSimpleKeySize keySize, int passWordIterations)
        {
            return RijndaelSimple.Encrypt(text, passPhrase, saltValue, hashAlgorithm.ToString(), passWordIterations, initVector, (int)keySize);
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="cipherText">The text to decrypt</param>
        /// <param name="passPhrase">The pass phrase used for this encryption</param>
        /// <param name="saltValue">The salt value used for this encryption</param>
        /// <param name="initVector">The init vector used for this encryption (must be 16 bit)</param>
        /// <param name="hashAlgorithm">The hash algorithm used for this encryption</param>
        /// <param name="keySize">The size of the key used for this encryption.</param>
        /// <param name="passWordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.</param>
        /// <returns></returns>
        public static string AES_Simple_Decrypt(string cipherText, string passPhrase, string saltValue
            , string initVector, RijndaelSimpleHashAlgorithm hashAlgorithm, RijndaelSimpleKeySize keySize, int passWordIterations)
        {
            return RijndaelSimple.Decrypt(cipherText, passPhrase, saltValue, hashAlgorithm.ToString(), passWordIterations, initVector, (int)keySize);
        }



        /// <summary>
        /// Encrypts a string using the AES algorithm and returns the
        /// base64 representation of that string.
        /// </summary>
        /// <param name="pTextToEncrypt"></param>
        /// <param name="pPassPhrase"></param>
        /// <param name="pInitVector"></param>
        /// <returns></returns>
        public static string AESEncrypt(string pTextToEncrypt, string pPassPhrase, string pInitVector)
        {
            RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(pPassPhrase, pInitVector);

            return rijndaelKey.Encrypt(pTextToEncrypt);
        }

        /// <summary>
        ///
        /// </summary>
        public enum AESHashAlgorithm : byte
        {
			/// <summary>
			/// MD5 algo
			/// </summary>
            MD5 = 0,
			/// <summary>
			/// SHA-1 algo
			/// </summary>
            SHA1 = 1,
        }
        /// <summary>
        /// AESs the decrypt.
        /// </summary>
        /// <param name="pTextToDecrypt">The text to decrypt</param>
        /// <param name="pPassPhrase">The pass phrase</param>
        /// <param name="pInitVector">The init vector</param>
        /// <param name="hashAlgorithm">The hashing algorithm to use</param>
        /// <returns></returns>
        public static string AESDecrypt(string pTextToDecrypt, string pPassPhrase, string pInitVector, AESHashAlgorithm hashAlgorithm)
        {
            RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(pPassPhrase, pInitVector, 4, 0, 128, hashAlgorithm.ToString());
            return rijndaelKey.Decrypt(pTextToDecrypt);
        }
        /// <summary>
        /// AESs the encrypt.
        /// </summary>
        /// <param name="pTextToEncrypt">The text to encrypt</param>
        /// <param name="pPassPhrase">The pass phrase</param>
        /// <param name="pInitVector">The init vector</param>
        /// <param name="hashAlgorithm">The hashing algorithm to use</param>
        /// <returns></returns>
        public static string AESEncrypt(string pTextToEncrypt, string pPassPhrase, string pInitVector, AESHashAlgorithm hashAlgorithm)
        {
            RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(pPassPhrase, pInitVector, 4, 0, 128, hashAlgorithm.ToString());
            return rijndaelKey.Encrypt(pTextToEncrypt);
        }

        /// <summary>
        /// Decrypts a base64 encoded string to its plaintext, using the specified passwords/IVs.
        /// </summary>
        /// <param name="pTextToDecrypt"></param>
        /// <param name="pPassPhrase"></param>
        /// <param name="pInitVector"></param>
        /// <returns></returns>
        public static string AESDecrypt(string pTextToDecrypt, string pPassPhrase, string pInitVector)
        {
            RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(pPassPhrase, pInitVector);

            return rijndaelKey.Decrypt(pTextToDecrypt);
        }

        /// <summary>
        /// Overloaded. Encrypts a string using AES and returns the cipher text of that string.
        /// </summary>
        /// <param name="pTextToEncrypt"></param>
        /// <param name="pPassPhrase"></param>
        /// <param name="pInitVector"></param>
        /// <returns></returns>
        public static byte[] AESEncryptToBytes(string pTextToEncrypt, string pPassPhrase, string pInitVector)
        {
            RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(pPassPhrase, pInitVector);

            return rijndaelKey.EncryptToBytes(pTextToEncrypt);
        }

        /// <summary>
        /// Overloaded. Decrypts a base64 encoded ciphertext using AES and returns the
        /// decrypted data.
        /// </summary>
        /// <param name="pTextToDecrypt"></param>
        /// <param name="pPassPhrase"></param>
        /// <param name="pInitVector"></param>
        /// <returns></returns>
        public static byte[] AESDecryptToBytes(string pTextToDecrypt, string pPassPhrase, string pInitVector)
        {
            RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(pPassPhrase, pInitVector);

            return rijndaelKey.DecryptToBytes(pTextToDecrypt);
        }

        /// <summary>
        /// Overloaded. Encrypts a block of data using AES and returns the encrypted data.
        /// </summary>
        /// <param name="pTextToEncrypt"></param>
        /// <param name="pPassPhrase"></param>
        /// <param name="pInitVector"></param>
        /// <returns></returns>
        public static byte[] AESEncryptToBytes(byte[] pTextToEncrypt, string pPassPhrase, string pInitVector)
        {
            RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(pPassPhrase, pInitVector);

            return rijndaelKey.EncryptToBytes(pTextToEncrypt);
        }

        /// <summary>
        /// Overloaded. Decrypts a cipher block and returns the decrypted data.
        /// </summary>
        /// <param name="pTextToDecrypt"></param>
        /// <param name="pPassPhrase"></param>
        /// <param name="pInitVector"></param>
        /// <returns></returns>
        public static byte[] AESDecryptToBytes(byte[] pTextToDecrypt, string pPassPhrase, string pInitVector)
        {
            RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(pPassPhrase, pInitVector);

            return rijndaelKey.DecryptToBytes(pTextToDecrypt);
        }

        /// <summary>
        /// Generates a random password of the exact length.
        /// </summary>
        /// <param name="pLength">
        /// Exact password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        public static string GeneratePassword(int pLength)
        {
            return RandomPassword.Generate(pLength, pLength);
        }

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <param name="pMinLength">
        /// Minimum password length.
        /// </param>
        /// <param name="pMaxLength">
        /// Maximum password length.
        /// </param>
        /// <returns>
        /// Randomly generated password.
        /// </returns>
        /// <remarks>
        /// The length of the generated password will be determined at
        /// random and it will fall with the range determined by the
        /// function parameters.
        /// </remarks>
        public static string GeneratePassword(int pMinLength, int pMaxLength)
        {
            return RandomPassword.Generate(pMinLength,
                            pMaxLength);
        }

        /* Changed this to a static class. */
    }
}
