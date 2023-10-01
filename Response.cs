namespace WebApplication2
{
    public class Response
    {
        public string ErrorMessage { get; set; }
        public object ReturnValue { get; set; }

        public bool ErrorOccured
            => ErrorMessage != null;
        public Response() { }


        public Response(object value)
        {
            ReturnValue = value;
        }


        public Response(string msg1)
        {
            ErrorMessage = msg1;
        }
        public Response(string msg1, object value)
        {
            ErrorMessage = msg1;
            ReturnValue = value;

        }
    }
}
