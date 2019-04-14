using BsonInspector.Core;
using BsonInspector.Core.Abstract;
using BsonInspector.Core.ValuePresenters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;

namespace BsonInspector.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBsonInsperctor _bsonInspector;

        public IndexModel(IBsonInsperctor bsonInspector)
        {
            _bsonInspector = bsonInspector;
        }

        [BindProperty]
        public string SuccessMessage { get; set; }

        [BindProperty]
        public string FailMessage { get; set; }

        [BindProperty]
        public IFormFile BsonFile { get; set; }

        public IActionResult OnPost()
        {
            if (BsonFile == null)
            {
                return ViewComponent("BsonDocument", new { model = new BsonInspectionResult("Please select file") });
            }

            try
            {
                using (var stream = BsonFile.OpenReadStream())
                {
                    var bson = ReadFully(stream);
                    var bsonInspectionResult = _bsonInspector.Inspect(bson);

                    return ViewComponent("BsonDocument", new { model = bsonInspectionResult });
                }
            }
            catch (Exception ex)
            {
                //TODO need informative display of what was wrong
                return Content(ex.Message);
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }

        }
    }
}
