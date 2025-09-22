using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class AutomaticExecutingConfiguration
    {
        public int Res_ExpirationHours { get; set; } = 24;
        public int Res_CheckIntervalMinutes { get; set; } = 5;
        public int BatchSize { get; set; } = 100;
    }
}
