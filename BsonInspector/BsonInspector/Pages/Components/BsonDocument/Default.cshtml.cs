using BsonInspector.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BsonInspector.Pages.Components.BsonDocument
{
    public class DefaultModel : PageModel
    {
        private readonly BsonInspectionResult _bsonInspectionResult;

        public DefaultModel(BsonInspectionResult bsonInspectionResult)
        {
            _bsonInspectionResult = bsonInspectionResult;
        }

        public Core.BsonDocument BsonDocument => _bsonInspectionResult.Document;

        public string ResultMessage => IsValid ?
                    "Your BSON is valid" :
                    $"Your BSON is invalid: {_bsonInspectionResult.Error}";

        public bool IsValid => _bsonInspectionResult.IsValid;
    }
}
