using System.Security.Cryptography;

namespace GvasViewer.FileFormat.Platform
{
	internal class FinalFantasyResonance : IFileFormat
	{
		// Create Key
		// "FFRS-Win64-Shipping.exe"+12CDC10 - 48 8B C4

		// Checksum
		// "FFRS-Win64-Shipping.exe"+134B000 - 40 53
		private UInt32 mVersion;
		private const String mKey = "";


		public byte[] Load(String filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			if (System.Text.Encoding.UTF8.GetString(buffer, 0, 4) != "SVHD") return [];

			mVersion = BitConverter.ToUInt32(buffer, 4);
			buffer = buffer[32..];
			buffer = Crypt(buffer, false);

			// 16Byte padding
			// Search "None"
			var index = buffer.Length - 8;
			for (; index >= 0; index--)
			{
				if (buffer[index] == 'N') break;
			}
			index += 8;

			return buffer[..index];
		}

		public void Save(String filename, Byte[] buffer)
		{
			// 16Byte padding
			Array.Resize(ref buffer, (buffer.Length + 15) / 16 * 16);

			buffer = Crypt(buffer, true);

			using var sha1 = SHA1.Create();
			buffer = [
				.. System.Text.Encoding.UTF8.GetBytes("SVHD"),
				.. BitConverter.GetBytes(mVersion),
				.. sha1.ComputeHash(buffer),
				.. BitConverter.GetBytes(0),
				.. buffer,
			];

			System.IO.File.WriteAllBytes(filename, buffer);
		}

		private Byte[] Crypt(Byte[] buffer, bool encrypt)
		{
			using var aes = Aes.Create();
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.None;
			aes.Key = System.Text.Encoding.UTF8.GetBytes(mKey);

			using var cryptor = encrypt
				? aes.CreateEncryptor()
				: aes.CreateDecryptor();

			return cryptor.TransformFinalBlock(buffer, 0, buffer.Length);
		}
	}
}
