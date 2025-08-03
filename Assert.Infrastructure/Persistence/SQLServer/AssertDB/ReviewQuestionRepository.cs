using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ReviewQuestionRepository : IReviewQuestionRepository
    {
        private readonly InfraAssertDbContext _context;
        public ReviewQuestionRepository(InfraAssertDbContext context)
        {
            _context = context;
        }

        public async Task<List<TReviewQuestion>> GetActiveQuestionsAsync()
        {
            return await _context.TReviewQuestions
                .Where(q => q.IsActive)
                .OrderBy(q => q.ReviewQuestionId)
                .ToListAsync();
        }

        public async Task<TReviewQuestion?> GetQuestionByIdAsync(int questionId)
        {
            return await _context.TReviewQuestions
                .FirstOrDefaultAsync(q => q.ReviewQuestionId == questionId);
        }
    }
}
