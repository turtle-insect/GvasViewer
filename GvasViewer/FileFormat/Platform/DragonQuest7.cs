using System.Security.Cryptography;

namespace GvasViewer.FileFormat.Platform
{
	internal class DragonQuest7 : IFileFormat
	{
		// Create Key
		// "DQ7R_DEMO-Win64-Shipping.exe"+1B815D9 - E8 A2FA0000
		//   - call "DQ7R_DEMO-Win64-Shipping.exe"+1B91080

		// Zlib
		// "DQ7R_DEMO-Win64-Shipping.exe"+AAFC8D - E8 2E530601
		//   - call "DQ7R_DEMO-Win64-Shipping.exe"+1B14FC0

		// Checksum
		// "DQ7R_DEMO-Win64-Shipping.exe"+15E2BB6 - E8 05825C00
		//   - call "DQ7R_DEMO-Win64-Shipping.exe"+1BAADC0
		// slicing-by-8 algorithm

		private Platform mPlatform;
		private UInt32 mVersion;
		// platform<version, AESKey>
		private readonly Dictionary<Platform, Dictionary<UInt32, String>> mKeys = new()
		{
			{
				Platform.Steam,
				new()
				{
					// Demo
					{ 0x0100, "SVKEY_AHj6kPzYp2BKD5-s63YAYLKXPH" },
				}
			},
			{
				Platform.Switch,
				new()
				{
					// Demo
					{ 0x0100, "SVKEY_SMu9925aiVxMsSncwbZFw_tY4K" },
				}
			}
		};

		public DragonQuest7(Platform platform)
		{
			mPlatform = platform;
		}

		public Byte[] Load(String filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			mVersion = BitConverter.ToUInt32(buffer);
			if (!mKeys[mPlatform].ContainsKey(mVersion)) return [];

			buffer = buffer[8..];
			using var aes = Aes.Create();
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.None;
			aes.Key = System.Text.Encoding.UTF8.GetBytes(mKeys[mPlatform][mVersion]);
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
			aes.Key = System.Text.Encoding.UTF8.GetBytes(mKeys[mPlatform][mVersion]);
			using var cryptor = aes.CreateEncryptor();
			buffer = cryptor.TransformFinalBlock(buffer, 0, buffer.Length);
			tmp = new Byte[buffer.Length + 8];
			Array.Copy(BitConverter.GetBytes(mVersion), tmp, 4);
			Array.Copy(BitConverter.GetBytes(buffer.Length), 0, tmp, 4, 4);
			Array.Copy(buffer, 0, tmp, 8, buffer.Length);
			buffer = tmp;

			System.IO.File.WriteAllBytes(filename, buffer);
		}
	}
}
