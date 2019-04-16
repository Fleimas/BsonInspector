using BsonInspector.Core.Abstract;

namespace BsonInspector.Core
{
    public class BsonElement
    {
        private readonly IValuePresenter _valuePresenter;

        public BsonElement(BsonElementTypes type, string name, BsonElementValue value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public BsonElementTypes Type { get; }
        public string Name { get; }
        public BsonElementValue Value { get; }
    }
}