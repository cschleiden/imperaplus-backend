using ImperaPlus.DTO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Web.Http.Validation;
using System;
using System.Web.Http.Metadata;

namespace ImperaPlus.Backend.Validation
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var error = new ErrorResponse(Application.ErrorCode.GenericApplicationError.ToString(), "Invalid input for method");

                error.Parameter_Errors = actionContext.ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
        }
    }

    public class PrefixlessBodyModelValidator : IBodyModelValidator
    {
        private readonly IBodyModelValidator _innerValidator;

        public PrefixlessBodyModelValidator(IBodyModelValidator innerValidator)
        {
            if (innerValidator == null)
            {
                throw new ArgumentNullException("innerValidator");
            }

            _innerValidator = innerValidator;
        }

        public bool Validate(object model, Type type, ModelMetadataProvider metadataProvider, HttpActionContext actionContext, string keyPrefix)
        {
            // Remove the keyPrefix but otherwise let innerValidator do what it normally does.
            return _innerValidator.Validate(model, type, metadataProvider, actionContext, String.Empty);
        }
    }
}