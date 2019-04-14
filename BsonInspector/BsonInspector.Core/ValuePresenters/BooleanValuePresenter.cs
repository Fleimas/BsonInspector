namespace BsonInspector.Core.ValuePresenters
{
    public class BooleanValuePresenter : IValuePresenter
    {
        private readonly byte _data;

        public BooleanValuePresenter(byte data)
        {
            _data = data;
        }

        public string Presentation()
        {
            return _data == 0 ? "false" : "true";
        }
    }
}
