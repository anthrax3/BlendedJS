using System;

namespace BlendedJS.Types
{
    public class Long : ValueType
    {
        private readonly object _value;

        public Long(object value)
        {
            _value = value;
        }

        public override object GetValue()
        {
            return Convert.ToInt64(_value);
        }
    }
}