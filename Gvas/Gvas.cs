using Gvas.Property;

namespace Gvas
{
	public class Gvas
	{
		public GvasEngine Engine { get; private set; } = new GvasEngine();
		public IList<GvasProperty> Properties { get; private set; } = new List<GvasProperty>();
		private Byte[] mFooter = [];
		public void Read(BinaryReader reader)
		{
			Properties.Clear();

			Engine.Read(reader);
			for(; ;)
			{
				var property = Util.Read(reader);
				Properties.Add(property);
				if (property is GvasNoneProperty) break;
			}

			// rest
			var length = reader.BaseStream.Length - reader.BaseStream.Position;
			mFooter = reader.ReadBytes((int)length);
		}

		public void Write(BinaryWriter writer)
		{
			Engine.Write(writer);
			foreach (var property in Properties)
			{
				property.Write(writer);
			}
			writer.Write(mFooter);
		}
	}
}
