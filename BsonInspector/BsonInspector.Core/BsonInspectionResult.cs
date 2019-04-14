using System;
using System.Collections.Generic;
using System.Text;

namespace BsonInspector.Core
{
    public class BsonInspectionResult
    {
        public BsonInspectionResult(BsonDocument document)
        {
            Document = document;
        }

        public BsonInspectionResult(string error)
        {
            Error = error;
            Document = null;
        }

        public bool IsValid => Document != null;
        public BsonDocument Document { get; }
        public string Error { get; }
    }
}
