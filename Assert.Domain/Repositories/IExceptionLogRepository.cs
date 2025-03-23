namespace Assert.Domain.Repositories
{
    public interface IExceptionLogRepository
    {
        Task SaveError(string message, string action, string module, object data, int? userId, string? ipAddress, string browseInfo);
        Task SaveException(Exception ex, string action, string module, object data, int? userId, string? ipAddress, string browseInfo);
    }
}
