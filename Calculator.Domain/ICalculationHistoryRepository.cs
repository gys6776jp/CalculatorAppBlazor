using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calculator.Domain
{
    public interface ICalculationHistoryRepository
    {
        Task AddAsync(CalculationHistory history);
        Task<List<CalculationHistory>> GetAllAsync();
    }
}
