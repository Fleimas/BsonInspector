using BsonInspector.Core.Abstract;
using System;

namespace BsonInspector.Core.ValuePresenters
{
    public class GUIDBinaryValuePresenter : IValuePresenter
    {
        private readonly byte[] _bytes;

        public GUIDBinaryValuePresenter(byte[] bytes)
        {
            _bytes = bytes;
        }

        public string Presentation()
        {
            return new Guid(_bytes).ToString();
        }
    }
}
