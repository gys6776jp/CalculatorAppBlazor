using Calculator.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.Infrastructure;

public class CalculationHistoryRepository : ICalculationHistoryRepository
{
    private readonly AppDbContext _context;

    public CalculationHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CalculationHistory history)
    {
        _context.CalculationHistories.Add(history);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CalculationHistory>> GetAllAsync()
    {
        return await _context.CalculationHistories
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}
