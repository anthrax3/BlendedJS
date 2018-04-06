using System;
using System.Collections.Generic;
using System.Text;

namespace BlendedJS.Types
{
    public class Int : ValueType
    {
        private readonly object _value;

        public Int(object value)
        {
            _value = value;
        }

        public override object GetValue()
        {
            return Convert.ToInt32(_value);
        }
    }
}
