namespace Gvas.Property
{
	public abstract class GvasProperty
	{
		public GvasString Name { get; set; } = new();
		private GvasProperty? _parent;
		private List<GvasProperty> _children = new();

		protected GvasProperty() { }
		protected GvasProperty(GvasProperty property)
		{
			Name = new(property.Name);
			foreach (var child in property._children)
			{
				AppendChildren(child.Clone());
			}
		}
		public abstract GvasProperty Clone();

		public IReadOnlyList<GvasProperty> Children
		{
			get => _children;
		}
		public void AppendChildren(GvasProperty property)
		{
			_children.Add(property);
			property._parent = this;
		}

		public void ClearChildren()
		{
			_children.Clear();
		}

		public String Path()
		{
			var names = new List<String>();
			names.Add(Name.Value);
			for (var parent = _parent; parent != null; parent = parent._parent)
			{
				names.Add(parent.Name.Value);
			}

			names.Reverse();
			return String.Join("\\", names);
		}

		public abstract Object Value { get; set; }
		public abstract void Read(BinaryReader reader);
		public abstract void Write(BinaryWriter writer);
		public abstract void ReadValue(BinaryReader reader);
		public abstract void WriteValue(BinaryWriter writer);
	}
}
