namespace BsonInspector.Core.ValuePresenters
{
    public class RegexValuePresenter : IValuePresenter
    {
        private readonly IValuePresenter _patternValuePresenter;
        private readonly IValuePresenter _optionsValuePresenter;

        public RegexValuePresenter(IValuePresenter patternValuePresenter, IValuePresenter optionsValuePresenter)
        {
            _patternValuePresenter = patternValuePresenter;
            _optionsValuePresenter = optionsValuePresenter;
        }

        public string Presentation()
        {
            return $"Pattern: {_patternValuePresenter.Presentation()}, Options: {_optionsValuePresenter}";
        }
    }
}
