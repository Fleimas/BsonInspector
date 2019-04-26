using System;
using System.Collections.Generic;

namespace BsonInspector.Core
{
    public class BsonDocument
    {
        public BsonDocument(int documentLength, byte[] valueInBytes, params BsonElement[] elements)
        {
            DocumentLength = documentLength;
            ValueInBytes = valueInBytes;
            foreach (var element in elements)
            {
                Elements.Add(element);
            }
        }

        public Guid Id { get; } = Guid.NewGuid();
        public IList<BsonElement> Elements { get; } = new List<BsonElement>();
        public int DocumentLength { get; }
        public byte[] ValueInBytes { get; }
    }
}
