namespace Gvas.Property
{
	public class GvasTree
	{
		private readonly List<GvasNode> _children = new();
		public IReadOnlyList<GvasNode> Children => _children;

		public GvasTree() { }

		public GvasTree(GvasTree tree)
		{
			foreach (var child in tree.Children)
			{
				GvasNode node = new();
				node.Name = new(child.Name);
				node.Tree = new(child.Tree);

				_children.Add(node);
			}
		}

		public void Read(BinaryReader reader)
		{
			var count = reader.ReadUInt32();

			for (var index = 0; index < count; index++)
			{
				GvasNode node = new();
				node.Name.Read(reader);
				node.Tree.Read(reader);

				_children.Add(node);
			}
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(_children.Count);

			foreach (var child in _children)
			{
				child.Name.Write(writer);
				child.Tree.Write(writer);
			}
		}
	}

	public class GvasNode
	{
		public GvasString Name { get; set; } = new();
		public GvasTree Tree { get; set; } = new();
	}
}
