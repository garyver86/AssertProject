using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ITErrorParamRepository
    {
        TErrorParam GetErrorByCode(string code);
        TErrorParam GetDefaultError();
        void LogException(TExceptionLog exceptionLog);
    }
}
