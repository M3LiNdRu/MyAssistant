using Library.MongoDb;

namespace MyAssistant.Apis.Expenses.Api.Resources.Categories
{ 
    public class Category : ICollectionDocument<int>
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
