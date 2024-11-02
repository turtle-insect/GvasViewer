namespace GvasViewer.Gvas
{
	internal abstract class GvasProperty
	{
		public uint Address { get; set; }
		public String Name { get; set; } = String.Empty;
		public uint Size { get; set; }
		public IList<GvasProperty> Children { get; set; } = new List<GvasProperty>();
		public abstract Object Value { get; set; }

		public abstract uint Read(uint address);
	}
}
