using System;

namespace Calculator.UseCase.Dto
{
    // 計算履歴を Web 層に渡すための DTO（Data Transfer Object）
    // - Domain のエンティティとほぼ同じ構造
    // - Web 層と Domain 層の依存関係を分離する目的
    public class CalculationHistoryDto
    {
        // 入力値 A
        public decimal A { get; set; }

        // 入力値 B
        public decimal B { get; set; }

        // 演算種別（add, subtract, multiply, divide）
        public string Operation { get; set; } = "";

        // 計算結果
        public decimal Result { get; set; }

        // 作成日時（UTC）
        public DateTime CreatedAt { get; set; }
    }
}
