using System;

namespace BlendedJS.Types
{
    public class Short : ValueType
    {
        private readonly object _value;

        public Short(object value)
        {
            _value = value;
        }

        public override object GetValue()
        {
            return Convert.ToInt16(_value);
        }
    }
}