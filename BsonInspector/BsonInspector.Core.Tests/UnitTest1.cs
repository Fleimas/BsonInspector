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

            var bson = generator.Generate(new { Id = Guid.NewGuid(), SveikasSk = 15, Tekstas = "Silvija", slenkantis=15.44, arne = false, dc = 300.545546545645111m });

            var inspected = inspector.Inspect(bson);
            Console.Write(new DocumentValuePresenter(inspected.Document).Presentation());
        }
    }
}
