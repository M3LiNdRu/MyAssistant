using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyAssistant.Apis.Expenses.Api.Resources.Historigrams
{
    [DataContract]
    public record SavingsHistorigram
    {
        public decimal TotalSavings => TotalEarned - TotalSpent;
        public decimal TotalEarned { get; set; } = 0M;
        public decimal TotalSpent { get; set; } = 0M;
        public Dictionary<DateTime, Dot> ProgressLine { get; set; } = new Dictionary<DateTime, Dot>();
    }

    [DataContract]
    public record Dot
    {
        public decimal Saved { get; set; }

        public decimal Spent { get; set; }

        public decimal Earned { get; set; }
    }
}
