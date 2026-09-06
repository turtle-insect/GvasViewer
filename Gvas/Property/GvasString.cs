using System.Text;

namespace Gvas.Property
{
	public class GvasString
	{
		public String Value { get; set; } = String.Empty;
		private Encoding _encoding = Encoding.UTF8;

		public GvasString() { }
		public GvasString(String value)
		{
			Value = value;
		}

		public GvasString(GvasString str)
			: this(str.Value)
		{
			this._encoding = str._encoding;
		}

		public void Read(BinaryReader reader)
		{
			int length = reader.ReadInt32();
			if (length == 0) return;

			var size = 1;
			if(length < 0)
			{
				size = 2;
				length = -length;
				_encoding = Encoding.Unicode;
			}

			var buffer = reader.ReadBytes((length - 1) * size);
			reader.ReadBytes(size);
			Value = _encoding.GetString(buffer);
		}

		public void Write(BinaryWriter writer)
		{
			int length = Value.Length;
			if (length == 0)
			{
				writer.Write(length);
				return;
			}

			length++;
			var count = 1;
			if (_encoding == Encoding.Unicode)
			{
				count = 2;
				length = -length;
			}

			writer.Write(length);
			writer.Write(_encoding.GetBytes(Value));
			foreach (var _ in Enumerable.Range(0, count))
			{
				writer.Write('\0');
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
