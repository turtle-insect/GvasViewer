namespace Gvas.Property
{
    internal class GvasUInt64Property : GvasProperty
    {
        public override object Value
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override uint Read(uint address)
        {
            uint length = 1;

            // value [1] -> 8Byte
            length += 8;

            return length;
        }
    }
}
