namespace GvasViewer.Gvas.Property
{
    class GvasStructProperty : GvasProperty
    {
        public override object Value
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override uint Read(uint address)
        {
            uint length = 0;

            var propName = Gvas.GetString(address + length);
            length += propName.length;

            // ???
            length += 17;

            length += ReadEntity(address + length, propName.name);
            return length;
        }

        public uint ReadEntity(uint address, string name)
        {
            Address = address;
            Name = name;
            uint length = 0;

            // if you make unique struct
            // extends IFileFormat implementation
            //   Create(uint address, String name)
            // function
            var fileFormat = SaveData.Instance().FileFormat;
            if (fileFormat != null)
            {
                length = fileFormat.Create(this, address + length, name);
                if (length != 0) return length;
            }

            switch (name)
            {
                case "Timespan":
                case "DateTime":
                case "Vector2D":
                    length += 8;
                    break;

                case "Color":
                    length += 4;
                    break;

                case "Vector":
                    length += 3 * 4;
                    break;

                default:
                    for (; ; )
                    {
                        var info = Gvas.Read(address + length);
                        length += info.length;
                        Children.Add(info.property);
                        if (info.property is GvasNoneProperty) break;
                    }
                    break;
            }

            return length;
        }
    }
}
