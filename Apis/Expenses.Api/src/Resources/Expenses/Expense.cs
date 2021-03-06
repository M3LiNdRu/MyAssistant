using System;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
    }

}