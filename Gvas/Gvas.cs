using Gvas.Property;
using Gvas.Property.Standard;

namespace Gvas
{
	public class Gvas
	{
		private GvasEngine mEngine = new();
		public List<GvasProperty> Properties { get; private set; } = new();
		private Byte[] mFooter = [];
		public void Read(BinaryReader reader)
		{
			Properties.Clear();

			mEngine.Read(reader);
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
			mEngine.Write(writer);
			foreach (var property in Properties)
			{
				property.Write(writer);
			}
			writer.Write(mFooter);
		}
	}
}
