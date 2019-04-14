using System;

namespace BsonInspector.Core.ValuePresenters
{
    public class IntValuePresenter : IValuePresenter
    {
        private readonly byte[] _data;

        public IntValuePresenter(byte[] data)
        {
            _data = data;
        }

        public string Presentation()
        {
            if (_data.Length == 4)
                return Convert.ToString(BitConverter.ToInt32(_data));

            if (_data.Length == 8)
                return Convert.ToString(BitConverter.ToInt64(_data));

            return Convert.ToString(_data);
        }
    }
}
