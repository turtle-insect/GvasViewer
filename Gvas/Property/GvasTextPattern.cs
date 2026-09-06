namespace Gvas.Property
{
	internal abstract class GvasTextPattern
	{
		public abstract GvasTextPattern Clone();
		public abstract String Value { get; set; }
		public abstract void Read(BinaryReader reader);
		public abstract void Write(BinaryWriter writer);
	}

	internal class GvasTextPatternNone : GvasTextPattern
	{
		private uint _flag;
		private GvasString _source = new();
		public override String Value
		{
			get => _source.Value;
			set => _source.Value = value;
		}

		public GvasTextPatternNone()
			: base()
		{ }

		public GvasTextPatternNone(GvasTextPatternNone pattern)
		{
			_flag = pattern._flag;
			_source = new(pattern._source);
		}

		public override GvasTextPattern Clone()
		{
			return new GvasTextPatternNone(this);
		}

		public override void Read(BinaryReader reader)
		{
			_flag = reader.ReadUInt32();
			if (_flag == 0) return;

			_source.Read(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			writer.Write(_flag);
			if (_flag == 0) return;

			_source.Write(writer);
		}
	}

	internal class GvasTextPatternNormal : GvasTextPattern
	{
		private GvasString _namespace = new();
		private GvasString _source = new();

		public override String Value
		{
			get => _source.Value;
			set => _source.Value = value;
		}

		public GvasTextPatternNormal()
			: base()
		{ }

		public GvasTextPatternNormal(GvasTextPatternNormal pattern)
		{
			_namespace = new(pattern._namespace);
			_source = new(pattern._source);
		}

		public override GvasTextPattern Clone()
		{
			return new GvasTextPatternNormal(this);
		}

		public override void Read(BinaryReader reader)
		{
			_namespace.Read(reader);
			_source.Read(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			_namespace.Write(writer);
			_source.Write(writer);
		}
	}

	internal class GvasTextPatternUnknown : GvasTextPattern
	{
		private readonly UInt64 _size;
		private Byte[] _buffer = [];

		public override String Value
		{
			get => String.Empty;
			set { }
		}

		public GvasTextPatternUnknown(UInt64 size)
			: base()
		{
			_size = size;
		}

		public GvasTextPatternUnknown(GvasTextPatternUnknown pattern)
		{
			_buffer = pattern._buffer.ToArray();
		}

		public override GvasTextPattern Clone()
		{
			return new GvasTextPatternUnknown(this);
		}

		public override void Read(BinaryReader reader)
		{
			_buffer = reader.ReadBytes((int)_size);
		}

		public override void Write(BinaryWriter writer)
		{
			writer.Write(_buffer);
		}
	}
}
