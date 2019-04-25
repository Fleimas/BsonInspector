using BsonInspector.Core.Abstract;
using BsonInspector.Core.Utility;
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
            var dateTime= DateTimeHelper.GetDateTimeFromUnixTicks(BitConverter.ToInt64(_data));
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ"); //ISO 8601
        }
    }
}
