using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Domain.Exceptions
{
    public class InvalidBondTypeNameException : Exception
    {
        public InvalidBondTypeNameException()
        {
        }

        public InvalidBondTypeNameException(string message)
            : base(message)
        {
        }

        public InvalidBondTypeNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
