namespace Assert.Application.DTOs.Responses
{
    public class DiscountDTO
    {
        public decimal DiscountCalculated { get; set; }

        public decimal Porcentage { get; set; }

        public bool IsDiscount { get; set; }

        public string Code { get; set; } = null!;

        public int Days { get; set; }

        public string Question { get; set; } = null!;

        public decimal PorcentageSuggest { get; set; }

        public string SuggestDescription { get; set; } = null!;
        public int? DiscountTypeForTypePriceId { get; set; }
    }
}