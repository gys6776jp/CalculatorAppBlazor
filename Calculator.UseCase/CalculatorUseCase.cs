using Calculator.Domain;
using Calculator.UseCase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.UseCase
{
    // 計算ユースケース
    // - Domain の Calculator を使って計算
    // - 計算結果を履歴として DB に保存
    // - Web 層向けに DTO で履歴を提供
    public class CalculatorUseCase
    {
        private readonly Calculator.Domain.Calculator _calculator;          // 四則演算処理クラス
        private readonly ICalculationHistoryRepository _repository;          // 履歴保存・取得用リポジトリ

        public CalculatorUseCase(ICalculationHistoryRepository repository)
        {
            _calculator = new Calculator.Domain.Calculator();
            _repository = repository;
        }

        // 計算 + DB保存を非同期でまとめる
        public async Task<decimal> CalculateAndSaveAsync(decimal a, decimal b, string operation)
        {
            // 四則演算を選択して結果を計算
            var result = operation switch
            {
                "add" => _calculator.Add(a, b),
                "subtract" => _calculator.Subtract(a, b),
                "multiply" => _calculator.Multiply(a, b),
                "divide" => b != 0 ? _calculator.Divide(a, b) : throw new DivideByZeroException(),
                _ => throw new ArgumentException("無効な演算種別です。")
            };

            try
            {
                // DB保存（非同期）
                await _repository.AddAsync(new CalculationHistory
                {
                    A = a,
                    B = b,
                    Operation = operation,
                    Result = result,
                    CreatedAt = DateTime.Now // UTC に変換は AppDbContext で対応済み
                });
            }
            catch (Exception ex)
            {
                // 例外を詳細にログ出力
                Console.WriteLine("Exception: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);

                // 必要なら再スローして呼び出し元でも捕まえられる
                throw;
            }

            return result;
        }

        // 計算履歴の取得（DTO で返す）
        public async Task<List<CalculationHistoryDto>> GetAllHistoriesAsync()
        {
            try
            {
                var histories = await _repository.GetAllAsync();

                // Domain のエンティティを Web 層向け DTO に変換
                return histories.Select(h => new CalculationHistoryDto
                {
                    A = h.A,
                    B = h.B,
                    Operation = h.Operation,
                    Result = h.Result,
                    CreatedAt = h.CreatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                // 例外を詳細にログ出力
                Console.WriteLine("GetAllHistoriesAsync Exception: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                throw;
            }
        }
    }
}
