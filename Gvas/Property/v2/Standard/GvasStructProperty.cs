namespace Gvas.Property.v2.Standard
{
	public class GvasStructProperty : GvasProperty
	{
		public GvasString Detail { get; set; } = new();
		private GvasTree _tree = new();
		private uint _version;
		private GvasString _guid = new();
		private Byte _flag;

		public GvasStructProperty()
			: base()
		{ }

		public GvasStructProperty(GvasStructProperty property)
			: base(property)
		{
			Detail = new(property.Detail);
			_tree = new(property._tree);
		}

		public override GvasProperty Clone()
		{
			return new GvasStructProperty(this);
		}

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			_version = reader.ReadUInt32();

			Detail.Read(reader);
			_tree.Read(reader);

			if(_version == 2)
			{
				_guid.Read(reader);
				reader.ReadUInt32();
			}

			// size
			var size = reader.ReadInt32();
			// flag
			_flag = reader.ReadByte();
			if(_flag != 0)
			{
				var property = new GvasLiteralProperty();
				property.Read(reader, size);
				AppendChildren(property);
			}
			else
			{
				ReadValue(reader);
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "StructProperty");

			using var ms = new MemoryStream();
			using var bw = new BinaryWriter(ms);
			WriteValue(bw);
			bw.Flush();

			writer.Write(_version);
			Detail.Write(writer);
			_tree.Write(writer);
			if (_version == 2)
			{
				_guid.Write(writer);
				writer.Write(0);
			}
			writer.Write((int)ms.Length);
			writer.Write(_flag);
			writer.Write(ms.ToArray());
		}

		public override void ReadValue(BinaryReader reader)
		{
			for (; ; )
			{
				var property = Util.ReadProperty(reader);
				property.Read(reader);
				AppendChildren(property);
				if (property is GvasNoneProperty) break;
			}
		}

		public override void WriteValue(BinaryWriter writer)
		{
			foreach (var child in Children)
			{
				child.Write(writer);
			}
		}
	}
}
