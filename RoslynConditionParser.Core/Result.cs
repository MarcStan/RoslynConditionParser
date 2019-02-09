namespace RoslynConditionParser.Core
{
    public class Result
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public bool Value { get; set; }

        public static Result Success(bool value)
        {
            return new Result
            {
                IsSuccess = true,
                Value = value
            };
        }

        public static Result Failure(string message)
        {
            return new Result
            {
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }
}
