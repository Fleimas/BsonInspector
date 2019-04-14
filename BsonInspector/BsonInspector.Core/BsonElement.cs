namespace BsonInspector.Core
{
    public class BsonElement
    {
        private readonly IValuePresenter _valuePresenter;

        public BsonElement(BsonElementTypes type, string name, byte[] value, IValuePresenter valuePresenter)
        {
            Type = type;
            Name = name;
            Value = value;
            _valuePresenter = valuePresenter;
        }

        public BsonElementTypes Type { get; }
        public string Name { get; }
        public byte[] Value { get; }

        public string PresentReadableValue()
        {
            return _valuePresenter.Presentation();
        }
    }
}