using GvasViewer.Gvas;
using GvasViewer.Util;

namespace GvasViewer.FileFormat
{
	// https://github.com/turtle-insect/UnrealEngineZlib
	internal class DivisionGvas : IFileFormat
	{
		public Byte[] Load(String filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			Byte[] output = Array.Empty<Byte>();
			for (int index = 0; index < buffer.Length;)
			{
				int size = BitConverter.ToInt32(buffer, index + 0x10);
				Byte[] comp = new Byte[size];
				Array.Copy(buffer, index + 0x30, comp, 0, comp.Length);

				Byte[] tmp = Zlib.Decompress(comp);
				Array.Resize(ref output, output.Length + tmp.Length);
				Array.Copy(tmp, 0, output, output.Length - tmp.Length, tmp.Length);
				if (BitConverter.ToInt32(buffer, index + 0x18) != 0x20000) break;
				index += size + 0x30;
			}

			// remove file size
			return output.Skip(4).ToArray();
		}

		public void Save(String filename, Byte[] buffer)
		{
			// append file size
			buffer = BitConverter.GetBytes(buffer.Length).Concat(buffer).ToArray();

			Byte[] output = Array.Empty<Byte>();
			for (int index = 0; index < buffer.Length; index += 0x20000)
			{
				int size = 0x20000;
				if (index + size > buffer.Length) size = buffer.Length - index;

				Byte[] decomp = new Byte[size];
				Array.Copy(buffer, index, decomp, 0, decomp.Length);
				Byte[] tmp = Zlib.Compression(decomp);
				int length = output.Length;
				Array.Resize(ref output, length + tmp.Length + 0x30);
				Array.Copy(BitConverter.GetBytes(0x9E2A83C1), 0, output, length, 4);
				Array.Copy(BitConverter.GetBytes(0x20000), 0, output, length + 8, 4);
				Array.Copy(BitConverter.GetBytes(tmp.Length), 0, output, length + 0x10, 4);
				Array.Copy(BitConverter.GetBytes(size), 0, output, length + 0x18, 4);
				Array.Copy(BitConverter.GetBytes(tmp.Length), 0, output, length + 0x20, 4);
				Array.Copy(BitConverter.GetBytes(size), 0, output, length + 0x28, 4);
				Array.Copy(tmp, 0, output, length + 0x30, tmp.Length);
			}

			System.IO.File.WriteAllBytes(filename, output);
		}

		public uint Create(GvasStructProperty property, uint address, String name)
		{
			return 0;
		}
	}
}
