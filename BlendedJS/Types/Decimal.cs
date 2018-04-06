using System;

namespace BlendedJS.Types
{
    public class Decimal : ValueType
    {
        private readonly object _value;

        public Decimal(object value)
        {
            _value = value;
        }

        public override object GetValue()
        {
            return Convert.ToDecimal(_value);
        }
    }
}