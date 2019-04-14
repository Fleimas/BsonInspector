using System;

namespace BsonInspector.Core.ValuePresenters
{
    public class FloatingPointValuePresenter : IValuePresenter
    {
        private readonly byte[] _data;

        public FloatingPointValuePresenter(byte[] data)
        {
            _data = data;
        }

        public string Presentation()
        {
            if (_data.Length == 8)
                return Convert.ToString(BitConverter.ToDouble(_data));

#warning TODO decimal 128
            if (_data.Length == 16)
                return Convert.ToString(_data);

            return Convert.ToString(_data);
        }
    }
}
