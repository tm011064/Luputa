using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace CommonTools.Components.Security
{
	/// <summary>
	/// A class that allows you to encrypt a file using crypt32's CryptProtectData function.
	/// </summary>
    public class DataProtector
    {        

		/// <summary>
		/// Provides information about the store to be used by CryptProtectData.
		/// Maps to the dwFlags element in that API.
		/// </summary>
        public enum Store
		{
			/// <summary>
			/// associates the encrypted data to the machine rather than the user.
			/// </summary>
			USE_MACHINE_STORE = 1,
			/// <summary>
			/// associates the encrypted data to the user rather than the machine.
			/// </summary>
			USE_USER_STORE = 0
		};
        private Store store;

		/// <summary>
		/// Initialises a new instance of DataProtector.
		/// </summary>
		/// <param name="tempStore">The registry key to use. Machine or user.</param>
        public DataProtector(Store tempStore)
        {
            store = tempStore;
        }

		/// <summary>
		/// Encrypts a block of data (with salt), using DPAPI and return the encrypted data.
		/// </summary>
		/// <param name="plainText">The text to encrypt</param>
		/// <param name="optionalEntropy">Any salt.</param>
		/// <returns>A DPAPI byte array containing the encrypted data.</returns>
        public byte[] Encrypt(byte[] plainText, byte[] optionalEntropy)
        {
            bool retVal = false;
			DATA_BLOB plainTextBlob = new DATA_BLOB();
			DATA_BLOB cipherTextBlob = new DATA_BLOB();
			DATA_BLOB entropyBlob = new DATA_BLOB();
			CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT();
            InitPromptstruct(ref prompt);
            int dwFlags;

            try
            {
                try
                {
                    int bytesSize = plainText.Length;
                    plainTextBlob.pbData = Marshal.AllocHGlobal(bytesSize);
                    if (IntPtr.Zero == plainTextBlob.pbData)
                    {
                        throw new Exception("Unable to allocate plaintext buffer.");
                    }
                    plainTextBlob.cbData = bytesSize;
                    Marshal.Copy(plainText, 0, plainTextBlob.pbData, bytesSize);
                }
                catch (Exception)
                {
                    throw;
                }

                if (Store.USE_MACHINE_STORE == store)
                {
                    //Using the machine store, should be providing entropy.
					dwFlags = SafeNativeMethods.CRYPTPROTECT_LOCAL_MACHINE | SafeNativeMethods.CRYPTPROTECT_UI_FORBIDDEN;

                    //Check to see if the entropy is null
                    if (null == optionalEntropy)
                    {
                        //Allocate something
                        optionalEntropy = new byte[0];
                    }

                    try
                    {
                        int bytesSize = optionalEntropy.Length;
                        entropyBlob.pbData = Marshal.AllocHGlobal(optionalEntropy.Length); ;
                        if (IntPtr.Zero == entropyBlob.pbData)
                        {
                            throw new Exception("Unable to allocate entropy data buffer.");
                        }
                        Marshal.Copy(optionalEntropy, 0, entropyBlob.pbData, bytesSize);
                        entropyBlob.cbData = bytesSize;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    //Using the user store
					dwFlags = SafeNativeMethods.CRYPTPROTECT_UI_FORBIDDEN;
                }

				retVal = SafeNativeMethods.CryptProtectData(
					ref plainTextBlob, "", ref entropyBlob,
					IntPtr.Zero, ref prompt, dwFlags,
					ref cipherTextBlob);
                if (false == retVal)
                {
                    throw new Win32Exception();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Encryption failed. See GetBaseException() for more details.", err);
            }

            byte[] cipherText = new byte[cipherTextBlob.cbData];
            Marshal.Copy(cipherTextBlob.pbData, cipherText, 0, cipherTextBlob.cbData);
            return cipherText;
        }

		/// <summary>
		/// Decrypts a block of data using the salt provided and DPAPI to return the decrypted data.
		/// </summary>
		/// <param name="cipherText">The text to encrypt</param>
		/// <param name="optionalEntropy">The salt used to decrypt the data.</param>
		/// <returns>A DPAPI byte array containing the encrypted data.</returns>
		public byte[] Decrypt(byte[] cipherText, byte[] optionalEntropy)
        {
            bool retVal = false;
			DATA_BLOB plainTextBlob = new DATA_BLOB();
			DATA_BLOB cipherBlob = new DATA_BLOB();
			CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT();
            InitPromptstruct(ref prompt);
            try
            {
                try
                {
                    int cipherTextSize = cipherText.Length;
                    cipherBlob.pbData = Marshal.AllocHGlobal(cipherTextSize);
                    if (IntPtr.Zero == cipherBlob.pbData)
                    {
                        throw new Exception("Unable to allocate cipherText buffer.");
                    }
                    cipherBlob.cbData = cipherTextSize;
                    Marshal.Copy(cipherText, 0, cipherBlob.pbData, cipherBlob.cbData);
                }
                catch (Exception)
                {
                    throw;
                }

				DATA_BLOB entropyBlob = new DATA_BLOB();
                int dwFlags;
                if (Store.USE_MACHINE_STORE == store)
                {
                    //Using the machine store, should be providing entropy.
					dwFlags = SafeNativeMethods.CRYPTPROTECT_LOCAL_MACHINE |
						SafeNativeMethods.CRYPTPROTECT_UI_FORBIDDEN;

                    //Check to see if the entropy is null
                    if (null == optionalEntropy)
                    {
                        //Allocate something
                        optionalEntropy = new byte[0];
                    }
                    try
                    {
                        int bytesSize = optionalEntropy.Length;
                        entropyBlob.pbData = Marshal.AllocHGlobal(bytesSize);
                        if (IntPtr.Zero == entropyBlob.pbData)
                        {
                            throw new Exception("Unable to allocate entropy buffer.");
                        }
                        entropyBlob.cbData = bytesSize;
                        Marshal.Copy(optionalEntropy, 0, entropyBlob.pbData, bytesSize);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    //Using the user store
					dwFlags = SafeNativeMethods.CRYPTPROTECT_UI_FORBIDDEN;
                }
				retVal = SafeNativeMethods.CryptUnprotectData(ref cipherBlob, null, ref entropyBlob,
	                IntPtr.Zero, ref prompt, dwFlags, ref plainTextBlob);
                if (false == retVal)
                {
                    throw new Win32Exception();
                }
                //Free the blob and entropy.
                if (IntPtr.Zero != cipherBlob.pbData)
                {
                    Marshal.FreeHGlobal(cipherBlob.pbData);
                }
                if (IntPtr.Zero != entropyBlob.pbData)
                {
                    Marshal.FreeHGlobal(entropyBlob.pbData);
                }
            }
            catch (Exception err)
            {
                throw new Exception("Decryption failed. See inner exception for details.", err);
            }

            byte[] plainText = new byte[plainTextBlob.cbData];
            Marshal.Copy(plainTextBlob.pbData, plainText, 0, plainTextBlob.cbData);
            return plainText;
        }

        private void InitPromptstruct(ref CRYPTPROTECT_PROMPTSTRUCT ps)
        {
			ps.cbSize = Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT));
            ps.dwPromptFlags = 0;
            ps.hwndApp = IntPtr.Zero;
            ps.szPrompt = null;
        }
    }
}
