using BsonInspector.Core.Abstract;
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

#warning TODO decimal 128, current imp probably invalid
            if (_data.Length == 16)
                return Convert.ToString(decimal.Parse(System.Text.Encoding.UTF8.GetString(_data).ToCharArray()));

            return Convert.ToString(_data);
        }
    }
}
