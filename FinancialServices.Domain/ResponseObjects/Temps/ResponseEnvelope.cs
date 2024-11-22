using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialServices.Domain.ResponseObjects.Temps
{
    public class ResponseEnvelope<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponseEnvelope()
        {
        }

        public ResponseEnvelope(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ResponseEnvelope<T> Ok(T data, string message = "")
        {
            return new ResponseEnvelope<T>(true, message, data);
        }

        public static ResponseEnvelope<T> Error(string message, T data)
        {
            return new ResponseEnvelope<T>(false, message, data);
        }
    }

}
