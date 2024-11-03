namespace GvasViewer.Gvas.Property
{
    internal class GvasEnumProperty : GvasProperty
    {
        public override object Value
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override uint Read(uint address)
        {
            uint length = 0;

            var propType = Gvas.GetString(address + length);
            length += propType.length;

            length++;

            var propName = Gvas.GetString(address + length);
            length += propName.length;

            return length;
        }
    }
}
