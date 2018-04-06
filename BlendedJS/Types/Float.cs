using System;

namespace BlendedJS.Types
{
    public class Float : ValueType
    {
        private readonly object _value;

        public Float(object value)
        {
            _value = value;
        }

        public override object GetValue()
        {
            return Convert.ToSingle(_value);
        }
    }
}