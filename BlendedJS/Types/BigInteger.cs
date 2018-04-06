namespace BlendedJS.Types
{
    public class BigInteger : ValueType
    {
        private readonly object _value;

        public BigInteger(object value)
        {
            _value = value;
        }

        public override object GetValue()
        {
            return new BigInteger(_value);
        }
    }
}