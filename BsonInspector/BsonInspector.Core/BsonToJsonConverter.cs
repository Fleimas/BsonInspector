using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.IO;

namespace BsonInspector.Core
{
    public class BsonToJsonConverter
	{
		private readonly JsonSerializer _serializer;

		public BsonToJsonConverter()
		{
			_serializer = new JsonSerializer();
		}

		public string ToJson(byte[] bson)
		{
			using (var stream = new MemoryStream(bson))
			using (var bsonReader = new BsonDataReader(stream))
			{
				var deserialized = _serializer.Deserialize(bsonReader);
				return deserialized.ToString();
			}
		}


	}
}
