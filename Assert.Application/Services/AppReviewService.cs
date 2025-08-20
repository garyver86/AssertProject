using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Models.Review;
using Assert.Domain.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Services
{
    public class AppReviewService : IAppReviewService
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppReviewService(IReviewService reviewService, IMapper mapper, IErrorHandler errorHandler)
        {
            _reviewService = reviewService;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }

        public async Task<ReturnModelDTO<List<ReviewQuestionDTO>>> GetReviewQuestions(Dictionary<string, string> clientData)
        {
            ReturnModelDTO<List<ReviewQuestionDTO>> result = new ReturnModelDTO<List<ReviewQuestionDTO>>();
            try
            {
                List<TReviewQuestion> returnModel = await _reviewService.GetReviewsQuestions();

                result = new ReturnModelDTO<List<ReviewQuestionDTO>>
                {
                    Data = _mapper.Map<List<ReviewQuestionDTO>>(returnModel),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppReviewService.GetReviewQuestions", ex, new { clientData }, true));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ReviewDTO>> GetBookReviewDetails(long bookId, Dictionary<string, string> clientData)
        {
            ReturnModelDTO<ReviewDTO> result = new ReturnModelDTO<ReviewDTO>();
            try
            {
                var returnModel = await _reviewService.GetReviewDetailsAsync(bookId);

                result = new ReturnModelDTO<ReviewDTO>
                {
                    Data = _mapper.Map<ReviewDTO>(returnModel),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppReviewService.GetBookReviewDetails", ex, new { bookId, clientData }, true));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ListingReviewResumeDTO>> GetreviewAverageByListing(long listingId, Dictionary<string, string> clientData)
        {
            ReturnModelDTO<ListingReviewResumeDTO> result = new ReturnModelDTO<ListingReviewResumeDTO>();
            try
            {
                var returnModel = await _reviewService.GetreviewAverageByListing(listingId);

                result = new ReturnModelDTO<ListingReviewResumeDTO>
                {
                    Data = _mapper.Map<ListingReviewResumeDTO>(returnModel),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppReviewService.GetreviewAverageByListing", ex, new { listingId, clientData }, true));
            }
            return result;
        }

        public async Task<ReturnModelDTO<ReviewDTO>> SubmitBookReview(ReviewDTO reviewDto, int userId, Dictionary<string, string> clientData)
        {
            ReturnModelDTO<ReviewDTO> result = new ReturnModelDTO<ReviewDTO>();
            try
            {
                var query = _mapper.Map<TlListingReview>(reviewDto);
                TlListingReview returnModel = await _reviewService.SubmitReviewAsync(query, userId);

                result = new ReturnModelDTO<ReviewDTO>
                {
                    Data = _mapper.Map<ReviewDTO>(returnModel),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppReviewService.SubmitBookReview", ex, new { reviewDto, userId, clientData }, true));
            }
            return result;
        }
    }
}
