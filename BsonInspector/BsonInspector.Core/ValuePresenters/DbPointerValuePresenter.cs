namespace BsonInspector.Core.ValuePresenters
{
    public class DbPointerValuePresenter : IValuePresenter
    {
        private readonly IValuePresenter _stringValuePresenter;
        private readonly IValuePresenter _objectIdValuePresenter;

        public DbPointerValuePresenter(IValuePresenter stringValuePresenter, IValuePresenter objectIdValuePresenter)
        {
            _stringValuePresenter = stringValuePresenter;
            _objectIdValuePresenter = objectIdValuePresenter;
        }

        public string Presentation()
        {
            return $"string: {_stringValuePresenter.Presentation()}, objectId: {_objectIdValuePresenter.Presentation()}";
        }
    }
}
