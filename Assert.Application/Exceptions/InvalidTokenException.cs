using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Exceptions;

public class InvalidTokenException : ApplicationException
{
    public InvalidTokenException(string message) : base(message) { }
}
