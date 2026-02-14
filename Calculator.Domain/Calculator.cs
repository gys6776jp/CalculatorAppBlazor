namespace Calculator.Domain;

// 電卓のドメインモデル
// - 四則演算を提供
// - 小数点計算対応
// - 0除算は例外を投げる
public class Calculator
{
    // 加算: a + b を計算して返す
    public decimal Add(decimal a, decimal b)
    {
        return a + b;
    }

    // 減算: a - b を計算して返す
    public decimal Subtract(decimal a, decimal b)
    {
        return a - b;
    }

    // 乗算: a * b を計算して返す
    public decimal Multiply(decimal a, decimal b)
    {
        return a * b;
    }

    // 除算: a / b を計算して返す
    // b が 0 の場合は DivideByZeroException を投げる
    public decimal Divide(decimal a, decimal b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("0で除算することはできません。");
        }
        return a / b;
    }
}
