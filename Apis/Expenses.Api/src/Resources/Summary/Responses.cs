using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyAssistant.Apis.Expenses.Api.Resources.Summary
{
    [DataContract]
    public record CurrentSummary
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public decimal TotalAmount { get; set; }
        public Dictionary<string, int> ProgressBar { get; set; } = new Dictionary<string, int>();
    }

    [DataContract]
    public record CompleteSummary
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public decimal Start { get; set; }
        public decimal Saved { get; set; }
        public Dictionary<string, decimal> SpentByCategory { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, int> ProgressBar { get; set; } = new Dictionary<string, int>();
    }
}
