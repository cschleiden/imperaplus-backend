using System.Collections.Generic;
using System.Threading.Tasks;
using NSwag;
using NSwag.SwaggerGeneration.Processors;
using NSwag.SwaggerGeneration.Processors.Contexts;

namespace ImperaPlus.Web
{
    public class SwaggerFormOperationProcessor : IOperationProcessor
    {
        public Task<bool> ProcessAsync(OperationProcessorContext context)
        {
            if (context.MethodInfo.Name.Contains("Exchange"))
            {
                context.OperationDescription.Operation.Consumes = new List<string> { "application/x-www-form-urlencoded" };
                //context.OperationDescription.Operation.Parameters.Clear();
                //context.OperationDescription.Operation.Parameters.Add(new SwaggerParameter
                //{
                //    Name = "grant_type",
                //    Type = NJsonSchema.JsonObjectType.String,
                //    IsRequired = true,
                //    Kind = SwaggerParameterKind.FormData
                //});
            }

            return Task.FromResult(true);
        }
    }
}
