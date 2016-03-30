using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace ADPhotoPrep
{
    internal static class Crypt
    {
        [DataProtectionPermission(SecurityAction.Demand, ProtectData = true)]
        internal static string EncryptString(string input)
        {
            byte[] bytes = null;
            byte[] encryptedData = null;

            try
            {//return EncryptString(input.ToSecureString());
                bytes = Encoding.Unicode.GetBytes(input);
                encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                    bytes, new byte[16],
                    System.Security.Cryptography.DataProtectionScope.CurrentUser
                );
                var res = Convert.ToBase64String(encryptedData);
                return res;
            }
            finally
            {
                Array.Clear(bytes, 0, bytes.Length);
                Array.Clear(encryptedData, 0, encryptedData.Length);
            }
        }

        internal static string EncryptString(this SecureString input)
        {
            return EncryptString(input.ToInsecureString());
        }

        [DataProtectionPermission(SecurityAction.Demand, UnprotectData = true)]
        internal static SecureString DecryptString(this string encryptedData)
        {
            byte[] bytes = null;
            byte[] decryptedData = null;

            try
            {
                bytes = Convert.FromBase64String(encryptedData);
                decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                        bytes,
                        new byte[16],
                        System.Security.Cryptography.DataProtectionScope.CurrentUser
                    );

                var res = ToSecureString(decryptedData);
                return res;
            }
            catch
            {
                return new SecureString();
            }
            finally
            {
                Array.Clear(decryptedData, 0, decryptedData.Length);
                Array.Clear(bytes, 0, bytes.Length);
            }
        }

        static SecureString ToSecureString(byte[] input)
        {
            SecureString secure = new SecureString();
            for (int i = 0; i < input.Length; i += 2)
            {
                char c = Encoding.Unicode.GetChars(input, i, 2).FirstOrDefault();
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        internal static SecureString ToSecureString(this string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
                secure.AppendChar(c);
            secure.MakeReadOnly();
            return secure;
        }

        internal static string ToInsecureString(this SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}
