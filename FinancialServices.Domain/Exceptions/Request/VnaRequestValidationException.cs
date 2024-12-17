using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Exceptions.Request
{
    public class VnaRequestValidationException : RequestValidationException
    {

        public VnaRequestValidationException(string message, List<string> validationErrors) : base(message, validationErrors)
        {
        }
    }
}
