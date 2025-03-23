using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface IErrorHandler
    {
        ErrorCommon GetError(string code, string additionalMessage, bool isTechnical);
        ErrorCommon GetErrorException(string action, Exception exception, object data, bool isTechnical);
        ErrorCommon RegisterSingleError(string action, string message, object data, string code, bool isTechnical);
    }
}
