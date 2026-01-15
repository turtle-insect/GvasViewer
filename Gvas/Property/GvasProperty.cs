namespace Gvas.Property
{
	public abstract class GvasProperty
	{
		public String Name { get; set; } = String.Empty;
		public List<GvasProperty> Childrens { get; init; } = new();

		protected GvasProperty() { }
		protected GvasProperty(GvasProperty property)
		{
			Name = property.Name;
			foreach (var child in property.Childrens)
			{
				Childrens.Add(child.Clone());
			}
		}

		public abstract Object Value { get; set; }
		public abstract GvasProperty Clone();
		public abstract void Read(BinaryReader reader);
		public abstract void Write(BinaryWriter writer);
		public abstract void WriteValue(BinaryWriter writer);
	}
}
