using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VendingMachine.Swagger
{
    public class SortMethodsCustomDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.OrderBy(e => e.Key).ToList();
            var openPaths = new OpenApiPaths();
            foreach (var (key, value) in paths)
            {
                openPaths.Add(key, value);
            }

            swaggerDoc.Paths = openPaths;
        }
    }
}
