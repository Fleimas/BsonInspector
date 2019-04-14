using BsonInspector.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BsonInspector.Pages.Components.BsonDocument
{
    public class BsonDocumentViewComponent : ViewComponent
    {
        public BsonDocumentViewComponent()
        {
        }

        public IViewComponentResult Invoke(BsonInspectionResult model)
        {           
            return View("Default", new DefaultModel(model) );
        }

    }
}
