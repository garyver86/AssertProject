﻿namespace Assert.Application.DTOs
{
    public class ReviewDTO
    {

        public string User { get; set; }
        public DateTime? DateTimeReview { get; set; }
        public int Calification { get; set; }
        public string? Comment { get; set; }
    }
}