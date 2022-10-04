using Microsoft.Extensions.Logging;
using MyAssistant.Apis.Expenses.Api.Resources.Expenses;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Summary
{
    public interface ISummaryService
    {
        Task<CurrentSummary> GetCurrentSummaryAsync(CancellationToken cancellationToken);
        Task<CompleteSummary> GetCompleteSummaryAsync(int year, int month, CancellationToken cancellationToken);
    }

    public class SummaryService : ISummaryService
    {
        private readonly IExpensesRepository expensesRepository;
        private readonly ILogger<IExpensesRepository> logger;

        public SummaryService(IExpensesRepository expensesRepository, ILogger<IExpensesRepository> logger)
        {
            this.expensesRepository = expensesRepository;
            this.logger = logger;
        }

        public async Task<CurrentSummary> GetCurrentSummaryAsync(CancellationToken cancellationToken)
        {
            var result = new CurrentSummary();
            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var expenses = await this.expensesRepository.GetByMonthAsync(firstDayOfMonth, cancellationToken);

            result.Year = firstDayOfMonth.Year.ToString();
            result.Month = firstDayOfMonth.ToString("MMMM");
            result.TotalAmount = expenses.Sum(expense => expense.Amount);
            var totalSpent = Math.Abs(expenses.Where(expense => expense.Amount < 0).Sum(expense => expense.Amount));

            foreach(var cat in expenses.Where(expense => expense.Amount < 0).GroupBy(expense => expense.Category))
            {
                var percentage = Convert.ToInt32(Math.Round(cat.Sum(ex => Math.Abs(ex.Amount) / totalSpent) * 100, 0));
                result.ProgressBar.Add(cat.Key, percentage);
            }

            return result;
        }

        public async Task<CompleteSummary> GetCompleteSummaryAsync(int year, int month, CancellationToken cancellationToken)
        {
            var result = new CompleteSummary();
            var firstDayOfMonth = new DateTime(year, month, 1);
            var expenses = await this.expensesRepository.GetByMonthAsync(firstDayOfMonth, cancellationToken);

            result.Year = firstDayOfMonth.Year.ToString();
            result.Month = firstDayOfMonth.ToString("MMMM");
            result.Saved = expenses.Sum(expense => expense.Amount);
            result.Start = expenses.Where(expense => expense.Amount > 0).Sum(expense => expense.Amount);
            var totalSpent = Math.Abs(expenses.Where(expense => expense.Amount < 0).Sum(expense => expense.Amount));

            foreach (var cat in expenses.Where(expense => expense.Amount < 0).GroupBy(expense => expense.Category))
            {
                var percentage = Convert.ToInt32(Math.Round((cat.Sum(ex => Math.Abs(ex.Amount)) / totalSpent) * 100, 0));
                result.ProgressBar.Add(cat.Key, percentage);
                result.SpentByCategory.Add(cat.Key, cat.Sum(ex => Math.Abs(ex.Amount)));
            }

            return result;
        }

    }
}
