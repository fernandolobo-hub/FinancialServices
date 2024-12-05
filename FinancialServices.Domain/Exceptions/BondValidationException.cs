using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Exceptions
{
    public class BondValidationException : Exception
    {
        public BondValidationException()
        {
        }

        public BondValidationException(string message)
            : base(message)
        {
        }

        public BondValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
