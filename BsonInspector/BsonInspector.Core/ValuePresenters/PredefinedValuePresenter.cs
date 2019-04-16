using BsonInspector.Core.Abstract;

namespace BsonInspector.Core.ValuePresenters
{
    public class PredefinedValuePresenter : IValuePresenter
    {
        private readonly string _value;

        public PredefinedValuePresenter(string value)
        {
            _value = value;
        }

        public string Presentation() => _value;
    }
}
