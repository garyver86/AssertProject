using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Shared.Extensions;

public static class ClassExtensions
{

    public static (string ClassFullName, string MethodName) GetCallerInfo(this object obj, int skipFrames = 2)
    {
        var stackTrace = new StackTrace(skipFrames, true);
        var method = stackTrace.GetFrame(0)?.GetMethod();

        if (method == null)
            return ("UnknownClass", "UnknownMethod");

        var classFullName = method.DeclaringType?.FullName ?? obj?.GetType().FullName ?? "UnknownClass";
        var methodName = method.Name;

        return (classFullName, methodName);
    }
}
