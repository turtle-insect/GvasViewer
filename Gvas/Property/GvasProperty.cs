namespace Gvas.Property
{
	public abstract class GvasProperty
	{
		public String Name { get; set; } = String.Empty;
		private GvasProperty? mParent;
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
			property.mParent = this;
		}

		public void ClearChildren()
		{
			mChildren.Clear();
		}

		public String Path()
		{
			var names = new List<String>();
			names.Add(Name);
			for (var parent = mParent; parent != null; parent = parent.mParent)
			{
				names.Add(parent.Name);
			}

			names.Reverse();
			return String.Join("\\", names);
		}

		public abstract Object Value { get; set; }
		public abstract void Read(BinaryReader reader);
		public abstract void Write(BinaryWriter writer);
		public abstract void WriteValue(BinaryWriter writer);
	}
}
