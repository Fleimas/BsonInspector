using BsonInspector.Core.Abstract;
using System;

namespace BsonInspector.Core.ValuePresenters
{
    public class UtcDateTimePresenter : IValuePresenter
    {
        private readonly byte[] _data;

        public UtcDateTimePresenter(byte[] data)
        {
            _data = data;
        }

        public string Presentation()
        {
            var dateTime = new DateTime(BitConverter.ToInt64(_data));
            return dateTime.ToString();
        }
    }
}
