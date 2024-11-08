namespace Gvas.Property
{
    internal class GvasFloatProperty : GvasProperty
    {
        public override object Value
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override uint Read(uint address)
        {
            uint length = 1;

            // value [1] -> 4Byte
            length += 4;

            return length;
        }
    }
}
