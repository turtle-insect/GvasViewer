using Gvas.Property;
using System.Text;

namespace Gvas
{
	public class GvasEngine
	{
		public GvasString Name { get; private set; } = new();

		private Byte[] _buffer = [];
		private String _header { get; set; } = String.Empty;
		private GvasString _detail { get; set; } = new();
		private readonly List<Guid> _guid = new();
		private uint _majorVersion = 0;
		private uint _minorVersion = 0;

		public void Read(BinaryReader reader)
		{
			Byte[] buffer = reader.ReadBytes(4);
			_header = Encoding.UTF8.GetString(buffer);

			if (_header != "GVAS") throw new Exception();

			var gameVersion = reader.ReadUInt32();

			reader.BaseStream.Position += 8;
			_majorVersion = reader.ReadUInt16();
			_minorVersion = reader.ReadUInt16();
			reader.BaseStream.Position += 2;

			if (gameVersion == 3) reader.BaseStream.Position += 4;

			_detail.Read(reader);

			// ???
			reader.BaseStream.Position += 4;

			uint count = reader.ReadUInt32();

			for (uint index = 0; index < count; index++)
			{
				buffer = reader.ReadBytes(16);
				reader.ReadInt32();
				_guid.Add(new Guid(buffer));
			}

			// data
			Name.Read(reader);

			// TODO
			long length = reader.BaseStream.Position;
			reader.BaseStream.Position = 0;
			_buffer = reader.ReadBytes((int)length);
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(_buffer);
		}

		public bool PropertyTag()
		{
			if (_majorVersion < 5) return false;
			if (_minorVersion < 4) return false;
			return true;
		}
	}
}
