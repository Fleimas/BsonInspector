namespace BsonInspector.Core.ValuePresenters
{
    public class GenericBinaryValuePresenter : IValuePresenter
    {
        private readonly byte[] _bytes;

        public GenericBinaryValuePresenter(byte[] bytes)
        {
            _bytes = bytes;
        }

        public string Presentation()
        {
            return "";
        }
    }
}
