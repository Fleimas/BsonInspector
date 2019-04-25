using BsonInspector.Core.Abstract;

namespace BsonInspector.Core
{
    public class BsonElement
    {
        public BsonElement(BsonElementTypes type, string name, BsonElementValue value, BinarySubtybes? subtybe = null)
        {
            Type = type;
            Name = name;
            Value = value;
            Subtybe = subtybe;
        }

        public BsonElementTypes Type { get; }


        public string Name { get; }
        public BsonElementValue Value { get; }

        public BinarySubtybes? Subtybe { get; }
    }
}