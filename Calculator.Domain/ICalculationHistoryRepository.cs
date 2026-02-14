using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calculator.Domain
{
    // 計算履歴リポジトリのインターフェイス（Repositoryパターン）
    // - 履歴の永続化や取得を抽象化
    // - Infrastructure層で具体的なDB実装に依存しない
    public interface ICalculationHistoryRepository
    {
        // 新しい計算履歴を非同期で追加する
        Task AddAsync(CalculationHistory history);

        // 全ての計算履歴を非同期で取得する
        Task<List<CalculationHistory>> GetAllAsync();
    }
}
