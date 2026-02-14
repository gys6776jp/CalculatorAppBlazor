using Calculator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Infrastructure
{
    // EF Core の DB コンテキスト
    // - DbSet でエンティティを定義
    // - OnModelCreating でテーブル名・カラム名を PostgreSQL に合わせて統一
    // - DateTime は UTC で保存するよう ValueConverter を設定
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 計算履歴テーブルへのアクセス
        public DbSet<CalculationHistory> CalculationHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // テーブル名を小文字＋アンダースコアに統一（PostgreSQL 用）
            modelBuilder.Entity<CalculationHistory>()
                .ToTable("calculation_histories");

            // 各カラム名を統一
            modelBuilder.Entity<CalculationHistory>()
                .Property(h => h.A)
                .HasColumnName("a");

            modelBuilder.Entity<CalculationHistory>()
                .Property(h => h.B)
                .HasColumnName("b");

            modelBuilder.Entity<CalculationHistory>()
                .Property(h => h.Operation)
                .HasColumnName("operation");

            modelBuilder.Entity<CalculationHistory>()
                .Property(h => h.Result)
                .HasColumnName("result");

            // 日時カラムを UTC として保存・取得する
            modelBuilder.Entity<CalculationHistory>()
                .Property(h => h.CreatedAt)
                .HasColumnName("created_at")
                .HasConversion(
                    v => v.ToUniversalTime(),                        // 保存時に UTC に変換
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)  // 取得時に UTC として扱う
                );
        }
    }
}
