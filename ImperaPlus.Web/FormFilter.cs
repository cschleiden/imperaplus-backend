using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;

namespace ImperaPlus.Web
{
    public class FormFilter : Swashbuckle.SwaggerGen.Generator.IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.Contains("Exchange"))
            {
                operation.Consumes.Add("application/x-www-form-urlencoded");

                foreach(var parameter in operation.Parameters)
                {
                    parameter.In = "formData";
                }
            };

            string actionName = ((ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ActionName;
            operation.OperationId = $"{context.ApiDescription.GroupName}_{actionName}";
        }
    }
}
