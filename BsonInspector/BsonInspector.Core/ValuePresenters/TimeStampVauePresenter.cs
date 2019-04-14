using System;

namespace BsonInspector.Core.ValuePresenters
{
    public class TimeStampVauePresenter : IValuePresenter
    {
        private readonly byte[] _data;

        public TimeStampVauePresenter(byte[] data)
        {
            _data = data;
        }

        public string Presentation() => Convert.ToString(BitConverter.ToDouble(_data));
    }
}
