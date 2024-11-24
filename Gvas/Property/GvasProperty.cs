namespace Gvas.Property
{
	public abstract class GvasProperty
	{
		public String Name { get; set; } = String.Empty;
		public IList<GvasProperty> Children { get; private set; } = new List<GvasProperty>();
		public abstract Object Value { get; set; }

		public abstract void Read(BinaryReader reader);
		public abstract void Write(BinaryWriter writer);
	}
}
