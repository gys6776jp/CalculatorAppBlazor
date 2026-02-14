using Calculator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet 名を統一
        public DbSet<CalculationHistory> CalculationHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Postgres に合わせてテーブル名を小文字＋アンダースコアに統一
            modelBuilder.Entity<CalculationHistory>()
                .ToTable("calculation_histories");

            // カラムも必要に応じて統一可能（省略可）
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

            modelBuilder.Entity<CalculationHistory>()
                .Property(h => h.CreatedAt)
                .HasColumnName("created_at")
                .HasConversion(
                    v => v.ToUniversalTime(),                        // 保存時 UTC に変換
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)  // 取得時 UTC として扱う
                );
        }
    }
}
