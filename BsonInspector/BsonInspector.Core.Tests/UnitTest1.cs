using BsonInspector.Core.ValuePresenters;
using System;
using Xunit;

namespace BsonInspector.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var converter = new BsonToJsonConverter();
            var generator = new BsonGenerator();

            var bson = generator.Generate(new { Id = Guid.NewGuid(), SveikasSk = 15, NesveikasNr = 11.56, Tekstas = "Silvija" });
            

            var resut = converter.ToJson(bson);
        }

        [Fact]
        public void InspectorTest()
        {
            var inspector = new BsonInspector();
            var generator = new BsonGenerator();

            var bson = generator.Generate(new { Id = Guid.NewGuid(), SveikasSk = 15, Tekstas = "Silvija", artaip = true, arne = false });

            var inspected = inspector.Inspect(bson);
            Console.Write(new DocumentValuePresenter(inspected.Document).Presentation());
        }
    }
}
