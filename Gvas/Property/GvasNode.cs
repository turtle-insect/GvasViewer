using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Gvas.Property
{
	internal class GvasNode
	{
		public List<GvasString> Names { get; } = new();
		private List<GvasNode> _children = new();

		public IReadOnlyList<GvasNode> Children
		{
			get => _children;
		}

		public GvasNode() { }

		public GvasNode(GvasNode node)
		{
			foreach(var name in node.Names)
			{
				GvasString tmp = new(name);
				Names.Add(tmp);
			}

			foreach (var child in node._children)
			{
				GvasNode tmp = new(child);
				_children.Add(tmp);
			}
		}

		public void Read(BinaryReader reader)
		{
			var count = reader.ReadUInt32();

			for (var index = 0; index < count; index++)
			{
				GvasString str = new();
				str.Read(reader);
				Names.Add(str);

				GvasNode child = new();
				_children.Add(child);
				child.Read(reader);
			}
		}

		public void Write(BinaryWriter writer)
		{
			var count = Names.Count;
			writer.Write(count);

			for (var index = 0; index < count; index++)
			{
				Names[index].Write(writer);
				_children[index].Write(writer);
			}
		}
	}
}
