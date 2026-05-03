using System.Diagnostics;
using System.Xml.Linq;

namespace Gvas.Property.v2.Standard
{
	public class GvasStructProperty : GvasProperty
	{
		public GvasString Detail { get; set; } = new();
		private GvasNode _node = new();
		private Byte _flag = 0;

		public GvasStructProperty()
			: base()
		{ }

		public GvasStructProperty(GvasStructProperty property)
			: base(property)
		{
			Detail = new(property.Detail);
			_node = new(property._node);
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
			reader.ReadUInt32();
			Detail.Read(reader);

			_node.Read(reader);

			// size
			var size = reader.ReadInt32();
			// flag
			_flag = reader.ReadByte();
			if(_flag == 8)
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

			writer.Write(1);
			Detail.Write(writer);
			_node.Write(writer);
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
