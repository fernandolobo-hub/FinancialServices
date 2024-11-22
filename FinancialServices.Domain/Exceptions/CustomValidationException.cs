using System;
using System.Collections.Generic;

namespace FinancialServices.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public List<string> ValidationErrors { get; }

        public CustomValidationException(string message, List<string> validationErrors) : base(message)
        {
            ValidationErrors = validationErrors;
        }

        public override string ToString()
        {
            return $"{Message}: {string.Join("; ", ValidationErrors)}";
        }
    }
}
