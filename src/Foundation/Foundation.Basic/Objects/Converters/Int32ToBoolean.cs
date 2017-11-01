using System;

namespace DreamCube.Foundation.Basic.Objects.Converters
{
    public class Int32ToBoolean : BasicConverter<Int32, Boolean>
    {
        public override Boolean Convert(Object inputValue)
        {
            return Utility.MyInt32.ToBoolean1(System.Convert.ToInt32(inputValue));
        }
    }
}
