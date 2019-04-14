using System.Text;

namespace BsonInspector.Core.ValuePresenters
{
    public class StringValuePresenter : IValuePresenter
    {
        private readonly byte[] _data;

        public StringValuePresenter(byte[] data)
        {
            _data = data;
        }

        public string Presentation()
        {
            return Encoding.UTF8.GetString(_data);
        }
    }
}
