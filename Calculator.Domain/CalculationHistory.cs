namespace Calculator.Domain;

// 計算履歴を表すエンティティクラス
public class CalculationHistory
{
    // 主キー
    public int Id { get; set; }

    // 計算の左辺の値
    public decimal A { get; set; }

    // 計算の右辺の値
    public decimal B { get; set; }

    // 演算子（add, subtract, multiply, divide など）
    public string Operation { get; set; } = "";

    // 計算結果
    public decimal Result { get; set; }

    // 計算日時（保存時は UTC にする想定）
    public DateTime CreatedAt { get; set; }
}
