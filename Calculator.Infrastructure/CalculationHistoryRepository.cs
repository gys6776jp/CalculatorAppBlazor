using Calculator.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.Infrastructure
{
    // 計算履歴のリポジトリ実装（DBアクセス層）
    // - ICalculationHistoryRepository を実装
    // - EF Core を利用して PostgreSQL にアクセス
    public class CalculationHistoryRepository : ICalculationHistoryRepository
    {
        private readonly AppDbContext _context;

        // コンストラクタ：DbContext を受け取る
        public CalculationHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        // 計算履歴の追加（非同期）
        public async Task AddAsync(CalculationHistory history)
        {
            // DbSet に追加
            _context.CalculationHistories.Add(history);

            // DB に保存
            await _context.SaveChangesAsync();
        }

        // 全履歴取得（非同期）
        public async Task<List<CalculationHistory>> GetAllAsync()
        {
            // 作成日時の降順で取得
            return await _context.CalculationHistories
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
