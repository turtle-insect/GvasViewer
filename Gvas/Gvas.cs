using Gvas.Property;
using Gvas.Property.Standard;

namespace Gvas
{
	public class Gvas
	{
		private GvasEngine mEngine = new();
		private List<GvasProperty> mProperties = new();
		private Byte[] mFooter = [];

		public IReadOnlyList<GvasProperty> Properties
		{
			get => mProperties;
		}

		public void Read(BinaryReader reader)
		{
			mProperties.Clear();

			mEngine.Read(reader);
			for(; ;)
			{
				var property = Util.Read(reader);
				mProperties.Add(property);
				if (property is GvasNoneProperty) break;
			}

			// rest
			var length = reader.BaseStream.Length - reader.BaseStream.Position;
			mFooter = reader.ReadBytes((int)length);
		}

		public void Write(BinaryWriter writer)
		{
			mEngine.Write(writer);
			foreach (var property in mProperties)
			{
				property.Write(writer);
			}
			writer.Write(mFooter);
		}
	}
}
