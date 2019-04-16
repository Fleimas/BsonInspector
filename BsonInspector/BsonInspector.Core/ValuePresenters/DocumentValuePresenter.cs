using BsonInspector.Core.Abstract;
using System.Text;

namespace BsonInspector.Core.ValuePresenters
{
    public class DocumentValuePresenter : IValuePresenter
    {
        private readonly BsonDocument _document;

        public DocumentValuePresenter(BsonDocument document)
        {
            _document = document;
        }

        public string Presentation()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Document length: {  _document.DocumentLength}");
            stringBuilder.AppendLine("Elements:");
            foreach (var element in _document.Elements)
            {
                stringBuilder.AppendLine($"{element.Name}, {element.Type}, {element.Value.HumanReadablePresenter.Presentation()}");
            }

            return stringBuilder.ToString();
        }
    }
}
