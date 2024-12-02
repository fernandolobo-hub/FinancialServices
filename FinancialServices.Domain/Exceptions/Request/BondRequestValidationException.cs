using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Exceptions.Request
{
    public class BondRequestValidationException : Exception
    {
        public List<string> ValidationErrors { get; }

        public BondRequestValidationException(string message, List<string> validationErrors) : base(message)
        {
            ValidationErrors = validationErrors;
        }

        public override string ToString()
        {
            return $"{Message} - {string.Join("; ", ValidationErrors)}";
        }
    }
}
