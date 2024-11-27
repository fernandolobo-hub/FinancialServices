using System;
using System.Collections.Generic;

namespace PublicBonds.Application.Exceptions
{
    public class DailyBondImportRequestValidationException : Exception
    {
        public List<string> ValidationErrors { get; }

        public DailyBondImportRequestValidationException(string message, List<string> validationErrors) : base(message)
        {
            ValidationErrors = validationErrors;
        }

        public override string ToString()
        {
            return $"{Message} - {string.Join("; ", ValidationErrors)}";
        }
    }
}
