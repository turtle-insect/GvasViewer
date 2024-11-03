namespace GvasViewer.Gvas.Property
{
    internal abstract class GvasProperty
    {
        public uint Address { get; set; }
        public string Name { get; set; } = string.Empty;
        public uint Size { get; set; }
        public IList<GvasProperty> Children { get; set; } = new List<GvasProperty>();
        public abstract object Value { get; set; }

        public abstract uint Read(uint address);
    }
}
