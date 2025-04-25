using Assert.Domain.Enums;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Mappings;

public class PlatformConverter : ITypeConverter<string, Platform>
{
    public Platform Convert(string source, Platform destination, 
        ResolutionContext context)
    {
        return Enum.TryParse(source, true, out Platform result) ? 
            result : Platform.None;
    }
}
