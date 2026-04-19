using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MyAssistant.Apis.Expenses.Api.Resources.Categories
{ 
    [DataContract]
    public record Request
    { 

        [Required]
        [DataMember(Name="name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name="description")]
        public string Description { get; set; }

    }
}
