using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CommonTools.Components.Security
{
	/// <summary>
	/// All Win32 DllImports go here.
	/// </summary>
	internal static class SafeNativeMethods
	{
		/// <summary>
		/// Log in interactively
		/// </summary>
		internal const int LOGON32_LOGON_INTERACTIVE = 2;

		/// <summary>
		/// Log in using the default.
		/// </summary>
		internal const int LOGON32_PROVIDER_DEFAULT = 0;

		[DllImport("advapi32.dll")]
		internal static extern int LogonUserA(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool RevertToSelf();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CloseHandle(IntPtr handle);

		[DllImport("Crypt32.dll", SetLastError = true,
			CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptProtectData
		(
			ref DATA_BLOB pDataIn,
			String szDataDescr,
			ref DATA_BLOB pOptionalEntropy,
			IntPtr pvReserved,
			ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
			int dwFlags,
			ref DATA_BLOB pDataOut
		);

		[DllImport("Crypt32.dll", SetLastError = true,
			CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CryptUnprotectData
		(
			ref DATA_BLOB pDataIn,
			String szDataDescr,
			ref DATA_BLOB pOptionalEntropy,
			IntPtr pvReserved,
			ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
			int dwFlags,
			ref DATA_BLOB pDataOut
		);

		internal const int CRYPTPROTECT_UI_FORBIDDEN = 0x1;
		internal const int CRYPTPROTECT_LOCAL_MACHINE = 0x4;

	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct DATA_BLOB
	{
		public int cbData;
		public IntPtr pbData;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CRYPTPROTECT_PROMPTSTRUCT
	{
		public int cbSize;
		public int dwPromptFlags;
		public IntPtr hwndApp;
		public String szPrompt;
	}

}
