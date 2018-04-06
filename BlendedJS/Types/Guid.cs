namespace BlendedJS.Types
{
    public class Guid : ValueType
    {
        private readonly object _value;

        public Guid(object value)
        {
            _value = value;
        }

        public override object GetValue()
        {
            return new Guid(_value);
        }
    }
}