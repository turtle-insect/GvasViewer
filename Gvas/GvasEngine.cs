using System.Text;

namespace Gvas
{
	public class GvasEngine
	{
		public String Header { get; private set; } = String.Empty;
		public String Name { get; private set; } = String.Empty;

		private Byte[] mBuffer = [];
		private readonly IList<Guid> mGuid = new List<Guid>();

		public void Read(BinaryReader reader)
		{
			Byte[] buffer = reader.ReadBytes(4);
			Header = Encoding.UTF8.GetString(buffer);

			if (Header != "GVAS") throw new Exception();

			var version = reader.ReadUInt32();
			reader.BaseStream.Position += 14;
			if (version == 3) reader.BaseStream.Position += 4;

			Name = Util.ReadString(reader);

			// ???
			reader.BaseStream.Position += 4;

			uint count = reader.ReadUInt32();

			for (uint index = 0; index < count; index++)
			{
				buffer = reader.ReadBytes(16);
				reader.ReadInt32();
				Guid guid = new Guid(buffer);
				mGuid.Add(guid);
			}

			// data
			Util.ReadString(reader);

			// TODO
			long length = reader.BaseStream.Position;
			reader.BaseStream.Position = 0;
			mBuffer = reader.ReadBytes((int)length);
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(mBuffer);
		}
	}
}
