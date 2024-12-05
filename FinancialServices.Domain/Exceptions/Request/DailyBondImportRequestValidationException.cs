using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicBonds.Domain.Exceptions.Request
{
    public class DailyBondImportRequestValidationException : RequestValidationException
    {
        public DailyBondImportRequestValidationException(string message, List<string> validationErrors) : base(message, validationErrors)
        {
        }
    }
}
