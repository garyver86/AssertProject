﻿using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IPayTransactionRepository
    {
        Task<long> Create(PayTransaction transaction);
    }
}
