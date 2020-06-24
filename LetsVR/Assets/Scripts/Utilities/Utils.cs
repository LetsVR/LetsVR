using System;
using System.Security.Cryptography;
using System.Text;

namespace LetsVR.XR.Utilities
{
	static class Utils
	{
		public static ulong Hash(this string mystring)
		{
			using (SHA256 sHA256 = SHA256.Create())
			{
				var hashed = sHA256.ComputeHash(Encoding.UTF8.GetBytes(mystring));
				return BitConverter.ToUInt64(hashed, 0);
			}
		}

		public static string GenerateClientName()
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			var byteArray = new byte[4];
			provider.GetBytes(byteArray);
			byteArray[1] = byteArray[2] = byteArray[3] = 0;

			//convert 4 bytes to an integer
			var randomInteger = BitConverter.ToUInt32(byteArray, 0);

			return $"Player{randomInteger}";
		}
	}
}
