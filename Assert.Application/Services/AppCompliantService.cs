using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assert.Application.Services
{
    public class AppCompliantService : IAppCompliantService
    {
        public readonly IMapper _mapper;
        public readonly RequestMetadata _metadata;
        public readonly IComplaintService _compliantService;
        public readonly IAppBookService _bookService;
        public AppCompliantService(IMapper mapper, RequestMetadata metadata, IComplaintService compliantService, IAppBookService bookService)
        {
            _mapper = mapper;
            _metadata = metadata;
            _compliantService = compliantService;
            _bookService = bookService;
        }
        public async Task<ReturnModelDTO<ComplaintDTO>> AddAsync(ComplaintRequestDTO entity)
        {
            var booking = _bookService.GetBookByIdAsync(entity.BookingId);
            TbComplaint data = new TbComplaint
            {
                BookingId = entity.BookingId,
                ComplainantUserId = _metadata.UserId,
                ComplaintStatusId = 1,
                ComplaintStatus = "PENDING",
                ComplaintDate = DateTime.UtcNow,
                ComplaintReasonId = entity.ComplaintReasonId,
                CreatedAt = DateTime.UtcNow,
                FreeTextDescription = entity.FreeTextDescription,
                IpAddress = _metadata.IpAddress,
                UserAgent = _metadata.UserAgent,
                ReportedHostId = (await booking).Data?.ListingRent?.OwnerUserId ?? 0,
                ComplainCode = $"CMP-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}"
            };
            await _compliantService.AddAsync(data);

            var result = await _compliantService.GetByBookingIdAsync(entity.BookingId);
            return new ReturnModelDTO<ComplaintDTO>
            {
                Data = _mapper.Map<ComplaintDTO>(result.Data),
                HasError = result.HasError,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError),
                StatusCode = result.StatusCode
            };
        }

        public async Task AssignComplaintAsync(int complaintId, int adminUserId)
        {
            await _compliantService.AssignComplaintAsync(complaintId, adminUserId);
        }

        public async Task<bool> ExistsForBookingAsync(int bookingId)
        {
            return await _compliantService.ExistsForBookingAsync(bookingId);
        }

        public async Task<ReturnModelDTO<ComplaintDTO>> GetByBookingIdAsync(int bookingId)
        {
            var result = await _compliantService.GetByBookingIdAsync(bookingId);
            return new ReturnModelDTO<ComplaintDTO>
            {
                Data = _mapper.Map<ComplaintDTO>(result.Data),
                HasError = result.HasError,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError),
                StatusCode = result.StatusCode
            };
        }

        public async Task<ReturnModelDTO<List<ComplaintDTO>>> GetByComplainantUserIdAsync(int userId)
        {
            var result = await _compliantService.GetByComplainantUserIdAsync(userId);
            return new ReturnModelDTO<List<ComplaintDTO>>
            {
                Data = _mapper.Map<List<ComplaintDTO>>(result.Data),
                HasError = result.HasError,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError),
                StatusCode = result.StatusCode
            };
        }

        public async Task<ReturnModelDTO<ComplaintDTO>> GetByComplaintCodeAsync(string complaintCode)
        {
            var result = await _compliantService.GetByComplaintCodeAsync(complaintCode);
            return new ReturnModelDTO<ComplaintDTO>
            {
                Data = _mapper.Map<ComplaintDTO>(result.Data),
                HasError = result.HasError,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError),
                StatusCode = result.StatusCode
            };
        }

        public async Task<ReturnModelDTO<List<ComplaintDTO>>> GetByReportedHostIdAsync(int hostId)
        {
            var result = await _compliantService.GetByReportedHostIdAsync(hostId);
            return new ReturnModelDTO<List<ComplaintDTO>>
            {
                Data = _mapper.Map<List<ComplaintDTO>>(result.Data),
                HasError = result.HasError,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError),
                StatusCode = result.StatusCode
            };
        }

        public async Task<ReturnModelDTO<List<ComplaintDTO>>> GetFilteredAsync(ComplaintFilterDto filter)
        {
            var result = await _compliantService.GetFilteredAsync(new ComplaintFilter
            {
                ComplainantUserId = filter.ComplainantUserId,
                ReportedHostId = filter.ReportedHostId,
                HostId = filter.HostId,
                StartDate = filter.StartDate,
                EndDate = filter.EndDate,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Priority = filter.Priority,
                Status = filter.Status,
                UserId = filter.UserId
            });
            return new ReturnModelDTO<List<ComplaintDTO>>
            {
                Data = _mapper.Map<List<ComplaintDTO>>(result.Data),
                HasError = result.HasError,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError),
                StatusCode = result.StatusCode
            };
        }

        public async Task<ReturnModelDTO<(List<ComplaintDTO>, PaginationMetadataDTO)>> GetPaginatedAsync(ComplaintFilterDto filter)
        {
            var result = await _compliantService.GetPaginatedAsync(new ComplaintFilter
            {
                ComplainantUserId = filter.ComplainantUserId,
                ReportedHostId = filter.ReportedHostId,
                HostId = filter.HostId,
                StartDate = filter.StartDate,
                EndDate = filter.EndDate,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Priority = filter.Priority,
                Status = filter.Status,
                UserId = filter.UserId
            });
            return new ReturnModelDTO<(List<ComplaintDTO>, PaginationMetadataDTO)>
            {
                Data = (_mapper.Map<List<ComplaintDTO>>(result.Data.Item1), _mapper.Map<PaginationMetadataDTO>(result.Data.Item2)),
                HasError = result.HasError,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError),
                StatusCode = result.StatusCode
            };
        }

        public async Task ResolveComplaintAsync(int complaintId, CompliantResolutionRequest resolutionInfo)
        {
            await _compliantService.ResolveComplaintAsync(complaintId, resolutionInfo.ResolutionType, resolutionInfo.Notes, resolutionInfo.internalNotes);
        }

        public async Task UpdateAsync(ComplaintRequestDTO entity)
        {
            await _compliantService.UpdateAsync(_mapper.Map<TbComplaint>(entity));
        }

        public async Task UpdateStatusAsync(int complaintId, string status, int? adminUserId = null)
        {
            await _compliantService.UpdateStatusAsync(complaintId, status, adminUserId);
        }
    }
}
