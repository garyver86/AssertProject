namespace Assert.Application.Exceptions;

public class InvalidTokenException : ApplicationException
{
    public InvalidTokenException(string message) : base(message) { }
}
