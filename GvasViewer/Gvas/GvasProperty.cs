namespace GvasViewer.Gvas
{
	internal abstract class GvasProperty
	{
		protected uint mAddress = 0;
		protected String mName = "";
		protected uint mSize = 0;

		public abstract uint Read(uint address);
		public abstract Object Value { get; set; }

		public uint Address
		{
			get => mAddress;
			set => mAddress = value;
		}

		public String Name
		{
			get => mName;
			set => mName = value;
		}

		public uint Size
		{
			get => mSize;
			set => mSize = value;
		}
	}
}
