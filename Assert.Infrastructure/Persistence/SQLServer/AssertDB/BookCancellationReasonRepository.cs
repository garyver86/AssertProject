using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Interfaces.Notifications;
using Assert.Domain.Repositories;
using Assert.Infrastructure.Exceptions;
using Assert.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class BookCancellationReasonRepository
    (IExceptionLoggerService _exceptionLoggerService,
    InfraAssertDbContext _dbContext,
    RequestMetadata _metadata)
    : IBookCancellationReasonRepository
{
    public async Task<List<TbBookCancellationReason>> GetFisrtStep(string cancellationTypeCode)
    {
        try
        {
            var result = await _dbContext.TbBookCancellationReasons
                .Include(g => g.CancellationGroup)
                .Where(x => x.CancellationLevel == 0
                    && x.CancellationTypeCode == cancellationTypeCode
                    && x.Status == "AC")
                .ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<List<TbBookCancellationReason>> GetNextStep(int cancellationReasonParentId)
    {
        try
        {
            var result = await _dbContext.TbBookCancellationReasons
                .Include(g => g.CancellationGroup)
                .Where(x => x.CancellationReasonParentId == cancellationReasonParentId
                    && x.Status == "AC")
                .ToListAsync();

            return result;
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }

    public async Task<List<TbBookCancellationReason>> GetParents(int cancellationReasonId)
    {
        try
        {
            var result = new List<TbBookCancellationReason>();
            await TraverseParents(cancellationReasonId, result);
            result.Reverse();
            return result;
        }
        catch (Exception ex)
        {
            var (className, methodName) = this.GetCallerInfo();
            _exceptionLoggerService.LogAsync(ex, methodName, className, new { _metadata.UserId });
            throw new InfrastructureException(ex.Message);
        }
    }


    private async Task TraverseParents(
        int cancellationReasonParentId, List<TbBookCancellationReason> list)
    {
        var cancellationParent = await _dbContext.TbBookCancellationReasons
                .Include(g => g.CancellationGroup)
                .Where(x => x.CancellationReasonId == cancellationReasonParentId
                    && x.Status == "AC")
                .FirstOrDefaultAsync();

        if (cancellationParent is null)
            throw new NotFoundException($"No existe motivo de cancelacion con identificador: {cancellationReasonParentId}");

        if (cancellationParent.CancellationGroup!.Position == 0)
            return;

        list.Add(cancellationParent);
        await TraverseParents(cancellationParent.CancellationReasonParentId!.Value, list);
    }
}
