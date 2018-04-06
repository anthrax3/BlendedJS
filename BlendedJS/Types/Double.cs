using System;

namespace BlendedJS.Types
{
    public class Double : ValueType
    {
        private readonly object _value;

        public Double(object value)
        {
            _value = value;
        }

        public override object GetValue()
        {
            return Convert.ToDouble(_value);
        }
    }
}