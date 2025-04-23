namespace Assert.Application.DTOs.Responses
{
    public class ReturnModelDTO
    {
        /// <summary>
        /// Estado de la solicitud (200 para éxito).
        /// </summary>
        public string? StatusCode { get; set; }
        /// <summary>
        /// Respuesta de la solicitud en caso de éxito.
        /// </summary>
        public object? Data { get; set; }
        public bool HasError { get; set; }
        public ErrorCommonDTO ResultError { get; set; }
    }
    public class ReturnModelDTO<T> : ReturnModelDTO
    {
        public new T? Data { get; set; }

        public ReturnModelDTO GetBase()
        {
            return new ReturnModelDTO
            {
                StatusCode = StatusCode,
                Data = Data,
                ResultError = ResultError,
                HasError = HasError
            };
        }
    }
    public class ErrorCommonDTO
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string? CorrelationId { get; set; }
    }
}
