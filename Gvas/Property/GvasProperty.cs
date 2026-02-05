namespace Gvas.Property
{
	public abstract class GvasProperty
	{
		public String Name { get; set; } = String.Empty;
		public GvasProperty? Parent { get; private set; }
		private List<GvasProperty> mChildren { get; init; } = new();

		protected GvasProperty() { }
		protected GvasProperty(GvasProperty property)
		{
			Name = property.Name;
			foreach (var child in property.mChildren)
			{
				AppendChildren(child.Clone());
			}
		}
		public abstract GvasProperty Clone();

		public IReadOnlyList<GvasProperty> Children
		{
			get => mChildren;
		}
		public void AppendChildren(GvasProperty property)
		{
			mChildren.Add(property);
			property.Parent = this;
		}

		public void ClearChildren()
		{
			mChildren.Clear();
		}

		public abstract Object Value { get; set; }
		public abstract void Read(BinaryReader reader);
		public abstract void Write(BinaryWriter writer);
		public abstract void WriteValue(BinaryWriter writer);
	}
}
