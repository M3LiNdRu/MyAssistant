using Library.MongoDb;
using System.Collections.Generic;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public class TagDocument : ICollectionDocument<int>
    {
        public int Id { get; set; }
        public List<string> Names { get; set; }
    }
}
