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
                    var bsonInspectionResult = _bsonInspector.Inspect(stream);
                    return ViewComponent("BsonDocument", new { model = bsonInspectionResult });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
