namespace BlendedJS.Types
{
    public abstract class ValueType
    {
        public abstract object GetValue();

        public override string ToString()
        {
            return GetValue().ToStringOrDefault();
        }
    }
}