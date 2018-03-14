namespace BlendedJS.Mongo
{
    public class ISODate : BaseObject
    {
        public string DateTimeString { get; set; }
        public ISODate(string dateTimeString)
        {
            DateTimeString = dateTimeString;
        }

        public override string ToString()
        {
            return DateTimeString;
        }
    }
}