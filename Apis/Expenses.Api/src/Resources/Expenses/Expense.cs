using Library.MongoDb;
using System;
using System.Collections.Generic;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public class Expense : ICollectionDocument<int>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public IList<string> Tags { get; set; }
    }

}