using BsonInspector.Core.Abstract;
using System;

namespace BsonInspector.Core
{
    public class BsonElementValue : IBsonElementValue
    {
        public BsonElementValue(byte[] bytes, IValuePresenter valuePresenter, BsonDocument innerDocument = null)
        {
            Bytes = bytes;
            HumanReadablePresenter = valuePresenter;
            Document = innerDocument;
        }

        public byte[] Bytes { get; }
        public IValuePresenter HumanReadablePresenter { get; }

        public bool IsDocument => Document != null;

        public BsonDocument Document { get; }
    }
}
