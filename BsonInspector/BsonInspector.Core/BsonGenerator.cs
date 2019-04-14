using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.IO;

namespace BsonInspector.Core
{
    public class BsonGenerator
	{
		private JsonSerializer _serializer;

		public BsonGenerator()
		{
			_serializer = new JsonSerializer();
		}

		public byte[] Generate(dynamic data)
		{
			using (var stream = new MemoryStream())
			using (var bsonWriter = new BsonDataWriter(stream))
			{
				_serializer.Serialize(bsonWriter, data);
				return stream.ToArray();
			}

		}
	}
}
