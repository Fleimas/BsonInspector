using BsonInspector.Core;
using BsonInspector.Core.Abstract;
using BsonInspector.Core.Utility;
using System;

namespace BsonInspector.Pages.Components.BsonDocument
{
    public class BsonDocumentViewModel
    {
        public BsonDocumentViewModel(Core.BsonDocument bsonDocument, bool isInnerDoc = false, bool isArray = false)
        {
            BsonDocument = bsonDocument;
            IsInnerDocument = isInnerDoc;
            IsArray = isArray;
        }

        public Core.BsonDocument BsonDocument { get; }
        public bool IsInnerDocument { get; }
        public bool IsArray { get; }

        public string LengthLabel
        {
            get
            {
                return IsArray ?
                    "Array value length" :
                    !IsInnerDocument ?
                        "BSON document length" :
                        "Inner document length";
            }
        }

        public string GetElementDataTypeDisplay(BsonElement element)
        {
            var dataType = EnumHelper<BsonElementTypes>.GetDisplayValue(element.Type);

            return element.Subtybe.HasValue ?
                    $"{dataType} ({EnumHelper<BinarySubtybes>.GetDisplayValue(element.Subtybe.Value)})"
                    : dataType;
        }

        public string GeHumanRepresentation(IValuePresenter presenter)
        {
            try
            {
                return presenter.Presentation();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}