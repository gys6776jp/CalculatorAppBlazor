namespace Calculator.Domain;

public class CalculationHistory
{
    public int Id { get; set; }
    public decimal A { get; set; }
    public decimal B { get; set; }
    public string Operation { get; set; } = "";
    public decimal Result { get; set; }
    public DateTime CreatedAt { get; set; }
}