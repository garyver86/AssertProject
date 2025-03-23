namespace Assert.Domain.Models
{
    public class ReturnModel
    {
        public string? StatusCode { get; set; }
        public object? Data { get; set; }
        public bool HasError { get; internal set; }
        public ErrorCommon ResultError { get; set; }
    }
    public class ReturnModel<T> : ReturnModel
    {
        public T? Data { get; set; }

        public ReturnModel GetBase()
        {
            return new ReturnModel
            {
                StatusCode = StatusCode,
                Data = Data,
                ResultError = ResultError,
                HasError = HasError
            };
        }
    }
}
