using System.Security.Cryptography;

namespace GvasViewer.FileFormat.Switch
{
	internal class DragonQuest7 : IFileFormat
	{
		// Dragon Quest VII Reimagined Demo
		const String AESKey = "SVKEY_AHj6kPzYp2BKD5-s63YAYLKXPH";
		public Byte[] Load(String filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			buffer = buffer[8..];

			using var aes = Aes.Create();
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.None;
			aes.Key = System.Text.Encoding.UTF8.GetBytes(AESKey);
			using var cryptor = aes.CreateDecryptor();
			buffer = cryptor.TransformFinalBlock(buffer, 0, buffer.Length);

			buffer = buffer[12..(BitConverter.ToInt32(buffer) + 12)];
			buffer = Util.Zlib.Decompress(buffer);

			return buffer;
		}

		public void Save(String filename, Byte[] buffer)
		{
			var length = buffer.Length;
			buffer = Util.Zlib.Compression(buffer);
			var size = (buffer.Length + 12 + 31) / 16 * 16;
			var tmp = new Byte[size];
			Array.Copy(BitConverter.GetBytes(buffer.Length), tmp, 4);
			Array.Copy(BitConverter.GetBytes(length), 0, tmp, 4, 4);
			Array.Copy(BitConverter.GetBytes(buffer.Length), 0, tmp, 8, 4);
			Array.Copy(buffer, 0, tmp, 12, buffer.Length);
			Array.Copy(BitConverter.GetBytes(buffer.Length + 12), 0, tmp, tmp.Length - 16, 4);
			var crc32 = new Util.Crc32(0xEDB88320);
			var sum = crc32.Calc(ref tmp, 0, buffer.Length + 12);
			Array.Copy(BitConverter.GetBytes(sum), 0, tmp, tmp.Length - 12, 4);
			buffer = tmp;

			using var aes = Aes.Create();
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.None;
			aes.Key = System.Text.Encoding.UTF8.GetBytes(AESKey);
			using var cryptor = aes.CreateEncryptor();
			buffer = cryptor.TransformFinalBlock(buffer, 0, buffer.Length);
			tmp = new Byte[buffer.Length + 8];
			tmp[1] = 0x01;
			Array.Copy(BitConverter.GetBytes(buffer.Length), 0, tmp, 4, 4);
			Array.Copy(buffer, 0, tmp, 8, buffer.Length);
			buffer = tmp;

			System.IO.File.WriteAllBytes(filename, buffer);
		}
	}
}
