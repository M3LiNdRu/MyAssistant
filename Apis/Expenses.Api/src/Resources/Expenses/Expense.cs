using System;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public class Expense
    {
        public int Id { get; internal set; }
        public DateTime Timestamp { get; internal set; }
        public string Category { get; internal set; }
        public string Name { get; internal set; }
        public float Amount { get; internal set; }
        public string Currency { get; internal set; }
    }

}