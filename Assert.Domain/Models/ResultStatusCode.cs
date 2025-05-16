using System.Net;

namespace Assert.Domain.Models
{
    public static class ResultStatusCode
    {
        public static string OK => GetStatusCode("OK");
        public static string BadRequest => GetStatusCode("BadRequest");
        public static string Unauthorized => GetStatusCode("Unauthorized");
        public static string Accepted => GetStatusCode("Accepted");
        public static string AlreadyReported => GetStatusCode("AlreadyReported");
        public static string Conflict => GetStatusCode("Conflict");
        public static string Continue => GetStatusCode("Continue");
        public static string Created => GetStatusCode("Created");
        public static string Forbidden => GetStatusCode("Forbidden");
        public static string NoContent => GetStatusCode("NoContent");
        public static string NotFound => GetStatusCode("NotFound");
        public static string PreconditionRequired => GetStatusCode("PreconditionRequired");
        public static string Redirect => GetStatusCode("Redirect");
        public static string RequestTimeout => GetStatusCode("RequestTimeout");
        public static string ServiceUnavailable => GetStatusCode("ServiceUnavailable");
        public static string InternalError => GetStatusCode("InternalError");
        public static string Error => "ERR";

        public static string NotFoundRecord = "El recurso solicitado no fue encontrado.";
        public static string NotFoundUpdate = "Recurso no encontrado, para actualización";
        public static string NotFoundDelete = "Recurso no encontrado, para eliminación";

        private static Dictionary<string, HttpStatusCode> statusCodeMapping = new Dictionary<string, HttpStatusCode>
        {
            { "OK", HttpStatusCode.OK },
            { "BadRequest", HttpStatusCode.BadRequest },
            { "Unauthorized", HttpStatusCode.Unauthorized },
            { "Accepted", HttpStatusCode.Accepted },
            { "AlreadyReported", HttpStatusCode.AlreadyReported },
            { "Conflict", HttpStatusCode.Conflict },
            { "Continue", HttpStatusCode.Continue },
            { "Created", HttpStatusCode.Created },
            { "Forbidden", HttpStatusCode.Forbidden },
            { "NoContent", HttpStatusCode.NoContent },
            { "NotFound", HttpStatusCode.NotFound },
            { "PreconditionRequired", HttpStatusCode.PreconditionRequired },
            { "Redirect", HttpStatusCode.Redirect },
            { "RequestTimeout", HttpStatusCode.RequestTimeout },
            { "ServiceUnavailable", HttpStatusCode.ServiceUnavailable },
            { "InternalError", HttpStatusCode.InternalServerError }
        };
        private static string GetStatusCode(string statusName)
        {
            if (statusCodeMapping.TryGetValue(statusName, out HttpStatusCode statusCode))
            {
                return ((int)statusCode).ToString();
            }
            else
            {
                throw new ArgumentException("Invalid status name");
            }
        }
    }
}
