using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IFuzzyMatcher
    {
        double Compare(string a, string b);
    }
}
