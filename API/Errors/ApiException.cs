namespace API.Errors
{
    public class ApiException : ApiResponse
    {     
        //For providing Stacktrace as third parameter for better understanding of an error
        public ApiException(int statusCode, string message = null, string details = null) 
            : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}