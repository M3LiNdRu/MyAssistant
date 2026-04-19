using System.Collections.Generic;
using System.Runtime.Serialization;


namespace MyAssistant.Apis.Expenses.Api.Resources.Categories 
{ 
    [DataContract]
    public record Response
    { 
        [DataMember(Name="categories")]
        public IEnumerable<Category> Categories { get; set; }
    }
}
