using System;

namespace Calculator.UseCase.Dto
{
    // Web 層に渡す DTO
    public class CalculationHistoryDto
    {
        public decimal A { get; set; }
        public decimal B { get; set; }
        public string Operation { get; set; } = "";
        public decimal Result { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
