using System.Collections.Generic;

namespace ImperaPlus.DTO
{
    /// <summary>
    /// Format for all returned errors
    /// </summary>
    public class ErrorResponse
    {
        private ErrorResponse()
        {
        }

        /// <summary>
        /// Create new instance of error response
        /// </summary>
        /// <param name="errorObject">Object to be converted to string</param>
        /// <param name="description">Error description</param>
        public ErrorResponse(object errorObject, string description)
            : this(errorObject.ToString(), description)
        {
        }

        /// <summary>
        /// Create new instance of error response
        /// </summary>
        /// <param name="error">Error identifier</param>
        /// <param name="description">Error description</param>
        public ErrorResponse(string error, string description)
        {
            Error = error;
            Error_Description = description;
        }

        /// <summary>
        /// Error code
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// English description of error
        /// </summary>
        public string Error_Description { get; private set; }

        /// <summary>
        /// Optional collection of errors
        /// </summary>
        public IDictionary<string, string[]> Parameter_Errors { get; set; }
    }
}
