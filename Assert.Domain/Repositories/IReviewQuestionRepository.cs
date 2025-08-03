using Assert.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Repositories
{
    public interface IReviewQuestionRepository
    {
        Task<List<TReviewQuestion>> GetActiveQuestionsAsync();
        Task<TReviewQuestion?> GetQuestionByIdAsync(int questionId);
    }
}
