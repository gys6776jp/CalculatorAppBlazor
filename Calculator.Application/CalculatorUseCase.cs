using Calculator.Domain;
using Calculator.UseCase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.UseCase
{
    public class CalculatorUseCase
    {
        private readonly Calculator.Domain.Calculator _calculator;
        private readonly ICalculationHistoryRepository _repository;

        public CalculatorUseCase(ICalculationHistoryRepository repository)
        {
            _calculator = new Calculator.Domain.Calculator();
            _repository = repository;
        }

        // 計算 + DB保存を非同期でまとめる
        public async Task<decimal> CalculateAndSaveAsync(decimal a, decimal b, string operation)
        {
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
                    CreatedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                // 例外内容を詳細に出力
                Console.WriteLine("Exception: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);

                // 必要なら再スローして呼び出し元でも捕まえられる
                throw;
            }

            return result;
        }

        // 履歴取得（DTO で返す）
        public async Task<List<CalculationHistoryDto>> GetAllHistoriesAsync()
        {
            try
            {
                var histories = await _repository.GetAllAsync();
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
                Console.WriteLine("GetAllHistoriesAsync Exception: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                throw;
            }
        }
    }
}
