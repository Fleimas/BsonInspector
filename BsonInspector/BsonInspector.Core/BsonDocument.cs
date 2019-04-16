using System;
using System.Collections.Generic;

namespace BsonInspector.Core
{
    public class BsonDocument
    {
        public BsonDocument(int documentLength, params BsonElement[] elements)
        {
            DocumentLength = documentLength;
            foreach (var element in elements)
            {
                Elements.Add(element);
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        public IList<BsonElement> Elements { get; } = new List<BsonElement>();

        public int DocumentLength { get; }
    }
}
