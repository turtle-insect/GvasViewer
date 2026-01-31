namespace Gvas.Property.Custom
{
	internal class GvasDQ7MemoryProperty : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public GvasDQ7MemoryProperty()
			: base()
		{ }

		public GvasDQ7MemoryProperty(GvasProperty property)
			: base(property)
		{
			throw new NotImplementedException();
		}

		public override GvasProperty Clone()
		{
			throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			Name = "Memory";

			AppendChildren(Util.Read(reader));
			AppendChildren(Util.Read(reader));

			// Memory
			//   ArrayProperty -> ByteProperty
			Util.ReadString(reader);
			Util.ReadString(reader);
			reader.ReadUInt64();
			Util.ReadString(reader);
			reader.ReadByte();
			reader.ReadUInt32();

			for (uint index = 0; index < 21; index++)
			{
				var property = Util.Read(reader);
				AppendChildren(property);
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Children[0].Write(writer);
			Children[1].Write(writer);

			Util.WriteString(writer, "Memory");
			Util.WriteString(writer, "ArrayProperty");

			using var ms = new MemoryStream();
			using var bw = new BinaryWriter(ms);
			for (int index = 2; index < Children.Count; index++)
			{
				Children[index].Write(bw);
			}
			bw.Flush();

			writer.Write(ms.Length + 4);
			Util.WriteString(writer, "ByteProperty");
			writer.Write('\0');
			writer.Write((UInt32)ms.Length);
			writer.Write(ms.ToArray());
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
