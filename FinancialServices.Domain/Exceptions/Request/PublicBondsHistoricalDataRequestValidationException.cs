using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Exceptions.Request
{
    public class PublicBondsHistoricalDataRequestValidationException : RequestValidationException
    {
        public PublicBondsHistoricalDataRequestValidationException(string message, List<string> validationErrors) : base(message, validationErrors)
        {
        }
    }
}
