namespace Gvas.Property.v2.Standard
{
	public class GvasTextProperty : GvasProperty
	{
		private GvasTextPattern _pattern = new GvasTextPatternNone();
		private UInt32 _flag;
		private Byte _type;

		public GvasTextProperty()
			: base()
		{ }

		public GvasTextProperty(GvasTextProperty property)
			: base(property)
		{
			_pattern = property._pattern.Clone();
			_flag = property._flag;
			_type = property._type;
		}

		public override GvasProperty Clone()
		{
			return new GvasTextProperty(this);
		}

		public override object Value
		{
			get => _pattern.Value;
			set => _pattern.Value = value.ToString() ?? "";
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			_flag = reader.ReadUInt32();
			_type = reader.ReadByte();

			switch (_type)
			{
				case 0x0b:
					_pattern = new GvasTextPatternNormal();
					break;

				case 0xFF:
					_pattern = new GvasTextPatternNone();
					break;

				default:
					_pattern = new GvasTextPatternUnknown(size - 5);
					break;
			}
			_pattern.Read(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "TextProperty");

			using var ms = new MemoryStream();
			using var bw = new BinaryWriter(ms);
			_pattern.Write(bw);
			bw.Flush();

			writer.Write(ms.Length + 5);
			writer.Write('\0');
			writer.Write(_flag);
			writer.Write(_type);
			writer.Write(ms.ToArray());
		}

		public override void ReadValue(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
