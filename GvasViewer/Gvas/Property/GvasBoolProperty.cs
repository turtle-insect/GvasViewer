namespace GvasViewer.Gvas.Property
{
    internal class GvasBoolProperty : GvasProperty
    {
        public override object Value
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override uint Read(uint address)
        {
            uint length = 0;

            // value [0] -> 2Byte
            length += 2;

            return length;
        }
    }
}
