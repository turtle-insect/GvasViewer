using System.Xml.Linq;

namespace Gvas.Property.v2.Standard
{
	public class GvasEnumProperty : GvasProperty
	{
		private GvasString mValue = new();
		private GvasNode _node = new();

		public GvasEnumProperty()
			: base()
		{ }

		public GvasEnumProperty(GvasEnumProperty property)
			: base(property)
		{
			mValue = new(property.mValue);
			_node = new(property._node);
		}

		public override GvasProperty Clone()
		{
			return new GvasEnumProperty(this);
		}

		public override object Value
		{
			get => mValue;
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			_node.Read(reader);

			// size
			reader.ReadUInt32();
			// flag
			reader.ReadByte();

			mValue.Read(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "EnumProperty");
			_node.Write(writer);
			writer.Write(mValue.Size() + 4);
			writer.Write('\0');
			mValue.Write(writer);
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
