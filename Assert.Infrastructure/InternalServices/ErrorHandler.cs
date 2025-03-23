using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Infrastructure.Utils;

namespace Assert.Infrastructure.InternalServices
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ITErrorParamRepository _errorRepository;

        public ErrorHandler(ITErrorParamRepository errorRepository)
        {
            _errorRepository = errorRepository;
        }

        public ErrorCommon GetError(string code, string additionalMessage, bool isTechnical)
        {
            if (code == ConstantsHelp.DB_1)
            {
                return new ErrorCommon
                {
                    Code = code,
                    Message = isTechnical ? "Ha ocurrido un problema con la conexión a la base de datos." : "A problem has occurred with the request.",
                    Title = "Error"
                };
            }
            else
            {
                var error = _errorRepository.GetErrorByCode(code) ?? _errorRepository.GetDefaultError();

                return new ErrorCommon
                {
                    Code = error.Code,
                    Message = string.Format(isTechnical ? error.TechnicalMessage : error.Message, additionalMessage),
                    Title = error.Title ?? "Error"
                };
            }
        }

        public ErrorCommon GetErrorException(string action, Exception exception, object data, bool isTechnical)
        {
            var error = _errorRepository.GetErrorByCode(ConstantsHelp.ERR_3);
            string jsonData = JsonMgr.ConvertToJson(data, 8000);

            try
            {
                var exceptionLog = new TExceptionLog
                {
                    Action = action,
                    DataRequest = jsonData,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    DateException = DateTime.Now
                };
                _errorRepository.LogException(exceptionLog);
            }
            catch
            {
                // Manejar el error de registro si es necesario
            }

            return new ErrorCommon
            {
                Code = error?.Code ?? ConstantsHelp.ERR_1,
                Message = string.Format(isTechnical ? error?.TechnicalMessage ?? "Internal Error" : error?.Message ?? "Internal Error", exception?.Message),
                Title = error?.Title ?? "Error"
            };
        }

        public ErrorCommon RegisterSingleError(string action, string message, object data, string code, bool isTechnical)
        {
            var error = _errorRepository.GetErrorByCode(code);
            string jsonData = JsonMgr.ConvertToJson(data, 8000);

            try
            {
                var exceptionLog = new TExceptionLog
                {
                    Action = action,
                    DataRequest = jsonData,
                    Message = message ?? "",
                    StackTrace = "",
                    DateException = DateTime.Now
                };
                _errorRepository.LogException(exceptionLog);
            }
            catch
            {
                // Manejar el error de registro si es necesario
            }

            return new ErrorCommon
            {
                Code = error?.Code ?? ConstantsHelp.ERR_1,
                Message = string.Format(isTechnical ? error?.TechnicalMessage ?? "Internal Error" : error?.Message ?? "Internal Error", message ?? "Internal Error"),
                Title = error?.Title ?? "Error"
            };
        }
    }
}
