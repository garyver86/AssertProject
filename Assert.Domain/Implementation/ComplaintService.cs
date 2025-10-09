using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Implementation
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IErrorHandler _errorHandler;
        public ComplaintService(IComplaintRepository complaintRepository, IErrorHandler errorHandler)
        {
            _complaintRepository = complaintRepository;
            _errorHandler = errorHandler;
        }

        public async Task AddAsync(TbComplaint entity)
        {
            ReturnModel<List<TComplaintReason>> result = new ReturnModel<List<TComplaintReason>>();
            try
            {
                await _complaintRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.AddAsync", ex, null, true);
            }
            return;
        }

        public async Task AssignComplaintAsync(int complaintId, int adminUserId)
        {
            ReturnModel<List<TComplaintReason>> result = new ReturnModel<List<TComplaintReason>>();
            try
            {
                await _complaintRepository.AssignComplaintAsync(complaintId, adminUserId);
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.AssignComplaintAsync", ex, null, true);
            }
            return;
        }

        public async Task<bool> ExistsForBookingAsync(long bookingId)
        {
            ReturnModel<List<TComplaintReason>> result = new ReturnModel<List<TComplaintReason>>();
            try
            {
                return await _complaintRepository.ExistsForBookingAsync(bookingId);
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.ExistsForBookingAsync", ex, null, true);
            }
            return false;
        }

        public async Task<ReturnModel<TbComplaint>> GetByBookingIdAsync(long bookingId)
        {
            ReturnModel<TbComplaint> result = new ReturnModel<TbComplaint>();
            try
            {
                return new ReturnModel<TbComplaint>
                {
                    Data = await _complaintRepository.GetByBookingIdAsync(bookingId),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.GetByBookingIdAsync", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TbComplaint>>> GetByComplainantUserIdAsync(int userId)
        {
            ReturnModel<List<TbComplaint>> result = new ReturnModel<List<TbComplaint>>();
            try
            {
                return new ReturnModel<List<TbComplaint>>
                {
                    Data = await _complaintRepository.GetByComplainantUserIdAsync(userId),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.GetByComplainantUserIdAsync", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<TbComplaint>> GetByComplaintCodeAsync(string complaintCode)
        {
            ReturnModel<TbComplaint> result = new ReturnModel<TbComplaint>();
            try
            {
                return new ReturnModel<TbComplaint>
                {
                    Data = await _complaintRepository.GetByComplaintCodeAsync(complaintCode),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.GetByComplaintCodeAsync", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TbComplaint>>> GetByReportedHostIdAsync(int hostId)
        {
            ReturnModel<List<TbComplaint>> result = new ReturnModel<List<TbComplaint>>();
            try
            {
                return new ReturnModel<List<TbComplaint>>
                {
                    Data = await _complaintRepository.GetByReportedHostIdAsync(hostId),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.GetByReportedHostIdAsync", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TbComplaint>>> GetFilteredAsync(ComplaintFilter filter)
        {
            ReturnModel<List<TbComplaint>> result = new ReturnModel<List<TbComplaint>>();
            try
            {
                return new ReturnModel<List<TbComplaint>>
                {
                    Data = await _complaintRepository.GetFilteredAsync(filter),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.GetFilteredAsync", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<(List<TbComplaint>, PaginationMetadata)>> GetPaginatedAsync(ComplaintFilter filter)
        {
            ReturnModel<(List<TbComplaint>, PaginationMetadata)> result = new ReturnModel<(List<TbComplaint>, PaginationMetadata)>();
            try
            {
                var result_ = await _complaintRepository.GetPaginatedAsync(filter);
                return new ReturnModel<(List<TbComplaint>, PaginationMetadata)>
                {
                    Data = (result_.Item1, result_.Item2),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("ComplaintService.GetPaginatedAsync", ex, null, true);
            }
            return result;
        }

        public async Task ResolveComplaintAsync(int complaintId, string resolutionType, string resolutionNotes, string internalNotes)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(TbComplaint entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateStatusAsync(int complaintId, string status, int? adminUserId = null)
        {
            throw new NotImplementedException();
        }
    }
}
