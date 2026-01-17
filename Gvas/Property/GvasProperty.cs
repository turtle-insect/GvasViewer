namespace Gvas.Property
{
	public abstract class GvasProperty
	{
		public String Name { get; set; } = String.Empty;
		protected List<GvasProperty> mChildrens { get; init; } = new();
		public IReadOnlyList<GvasProperty> Childrens
		{
			get => mChildrens;
		}

		protected GvasProperty() { }
		protected GvasProperty(GvasProperty property)
		{
			Name = property.Name;
			foreach (var child in property.mChildrens)
			{
				mChildrens.Add(child.Clone());
			}
		}

		public abstract Object Value { get; set; }
		public abstract GvasProperty Clone();
		public abstract void Read(BinaryReader reader);
		public abstract void Write(BinaryWriter writer);
		public abstract void WriteValue(BinaryWriter writer);

		public void AppendChildren(GvasProperty children)
		{
			mChildrens.Add(children);
		}
	}
}
