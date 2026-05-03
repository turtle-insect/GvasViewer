using System.Text;

namespace Gvas.Property
{
	public class GvasString
	{
		public String Value { get; set; } = String.Empty;
		private Encoding _encoding = Encoding.UTF8;

		public GvasString() { }
		public GvasString(String value, Encoding encoding)
		{
			Value = value;
			_encoding = encoding;
		}
		public GvasString(GvasString str)
			: this(str.Value, str._encoding) { }

		public void Read(BinaryReader reader)
		{
			int length = reader.ReadInt32();
			if (length == 0) return;

			if (length < 0)
			{
				length = -length - 1;
				var buffer = reader.ReadBytes(length * 2);
				reader.ReadBytes(2);
				_encoding = Encoding.Unicode;
				Value = _encoding.GetString(buffer);
			}
			else
			{
				var buffer = reader.ReadBytes(length - 1);
				reader.ReadByte();
				_encoding = Encoding.UTF8;
				Value = _encoding.GetString(buffer);
			}
		}

		public void Write(BinaryWriter writer)
		{
			int length = Value.Length;
			if (length != 0) length++;

			if(_encoding == Encoding.Unicode)
			{
				length = -length;
				writer.Write(length);
				if (length != 0)
				{
					var tmp = _encoding.GetBytes(Value);
					writer.Write(tmp);
					writer.Write('\0');
					writer.Write('\0');
				}
			}
			else
			{
				writer.Write(length);
				if(length != 0)
				{
					writer.Write(_encoding.GetBytes(Value));
					writer.Write('\0');
				}
			}
		}

		public uint Size()
		{
			var length = _encoding.GetBytes(Value).Length;
			if (length == 0) return 0;

			return (uint)length + (uint)(_encoding == Encoding.Unicode ? 2 : 1);
		}
	}
}
