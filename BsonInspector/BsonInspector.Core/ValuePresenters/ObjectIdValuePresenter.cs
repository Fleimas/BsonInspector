using BsonInspector.Core.Abstract;
using System;

namespace BsonInspector.Core.ValuePresenters
{
    public class ObjectIdValuePresenter : IValuePresenter
    {
        private readonly byte[] _data;

        public ObjectIdValuePresenter(byte[] data)
        {
            _data = data;
        }

        public string Presentation()
        {
            throw new NotImplementedException();
        }
    }
}
