using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Exceptions;

public class DatabaseUnavailableException : InfrastructureException
{
    public DatabaseUnavailableException(string? message = null)
        : base(message ?? "Database connection failed.") { }
}
