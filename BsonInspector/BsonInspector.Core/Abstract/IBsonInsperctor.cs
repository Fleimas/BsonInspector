using System.IO;

namespace BsonInspector.Core.Abstract
{
    public interface IBsonInsperctor
    {
        BsonInspectionResult Inspect(Stream bson);
    }
}
