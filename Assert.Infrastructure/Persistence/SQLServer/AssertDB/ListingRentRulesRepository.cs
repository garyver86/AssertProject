using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingRentRulesRepository : IListingRentRulesRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingRentRulesRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingRentRule>?> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingRentRules.Where(x => x.ListingId == listingRentId).ToListAsync();
            return result;
        }
        public async Task Set(long listingRentId, List<int> rules)
        {
            var actualRules = _context.TlListingRentRules.Where(x => x.ListingId == listingRentId).ToList();
            List<int> alreadyExist = new List<int>();
            foreach (var rule in actualRules)
            {
                if (!rules.Contains(rule.RuleTypeId ?? 0))
                {
                    _context.TlListingRentRules.Remove(rule);
                }
                else
                {
                    alreadyExist.Add(rule.RuleTypeId ?? 0);
                }
            }
            foreach (var rule in rules)
            {
                if (!alreadyExist.Contains(rule))
                {
                    TlListingRentRule newRule = new TlListingRentRule
                    {
                        RuleTypeId = rule,
                        ListingId = listingRentId
                    };
                    _context.TlListingRentRules.Add(newRule);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
