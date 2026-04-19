using Microsoft.Extensions.Logging;
using MongoDB.Driver.Linq;
using MyAssistant.Apis.Expenses.Api.Resources.Expenses;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Historigrams
{
    public interface IHistorigramService
    {
        Task<SavingsHistorigram> GetSavingsHistorigramAsync(CancellationToken cancellationToken);
    }

    public class HistorigramService : IHistorigramService
    {
        private readonly IExpensesRepository expensesRepository;
        private readonly ILogger<IExpensesRepository> logger;

        public HistorigramService(IExpensesRepository expensesRepository, ILogger<IExpensesRepository> logger)
        {
            this.expensesRepository = expensesRepository;
            this.logger = logger;
        }

        public async Task<SavingsHistorigram> GetSavingsHistorigramAsync(CancellationToken cancellationToken)
        {
            var result = new SavingsHistorigram();
            var expenses = await this.expensesRepository.GetAsync(cancellationToken);
            
            if (expenses != null)
            {
                result.TotalSpent = Math.Abs(expenses.Where(expense => expense.Amount < 0).Sum(expense => expense.Amount));
                result.TotalEarned = Math.Abs(expenses.Where(expense => expense.Amount > 0).Sum(expense => expense.Amount));

                var saved = 0M;
                foreach(var month in expenses.Where(e => e.Timestamp >= DateTime.Today.AddMonths(-11)).GroupBy(m => m.Timestamp.Month))
                {
                    var spent = Math.Abs(month.Where(expense => expense.Amount < 0).Sum(expense => expense.Amount));
                    var earned = Math.Abs(month.Where(expense => expense.Amount > 0).Sum(expense => expense.Amount));
                    saved += (earned - spent);
                    var date = new DateTime(month.First().Timestamp.Year, month.First().Timestamp.Month, 1);
                    result.ProgressLine.Add(date, new Dot { Earned = earned, Saved = saved, Spent = spent });
                }

            }

            return result;
        }

    }
}
