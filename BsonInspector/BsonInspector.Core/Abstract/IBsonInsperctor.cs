namespace BsonInspector.Core.Abstract
{
    public interface IBsonInsperctor
    {
        BsonInspectionResult Inspect(byte[] bson);
    }
}
