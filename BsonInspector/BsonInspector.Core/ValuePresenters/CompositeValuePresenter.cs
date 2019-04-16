using BsonInspector.Core.Abstract;
using System.Linq;

namespace BsonInspector.Core.ValuePresenters
{
    public class CompositeValuePresenter : IValuePresenter
    {
        private readonly IValuePresenter[] _presenters;

        public CompositeValuePresenter(params IValuePresenter[] presenters)
        {
            _presenters = presenters;
        }

        public string Presentation()
        {
            return string.Join('|', _presenters.Select(p => p.Presentation()));
        }
    }
}
