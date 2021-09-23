namespace RussianQuiz.Connect.Functions.Models.Responses
{
    public class BaseErrorResponse
    {
        public string Message { get; set; }

        public BaseErrorResponse() { }

        public BaseErrorResponse(string message)
        {
            Message = message;
        }
    }
}