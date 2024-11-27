namespace PublicBonds.Domain.ResponseObjects.Temps
{
    public class ResponseEnvelope<T>(bool success, string message, T? data)
    {
        public bool Success { get; set; } = success;
        public string Message { get; set; } = message;
        public T? Data { get; set; } = data;

        public static ResponseEnvelope<T> Ok(T? data = default, string message = "")
        {
            return new ResponseEnvelope<T>(true, message, data);
        }

        public static ResponseEnvelope<T> Error(T? data = default, string message = "")
        {
            return new ResponseEnvelope<T>(false, message, data);
        }
    }

}
