using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MyAssistant.Apis.Expenses.Api.Resources.Tags
{ 

    [DataContract]
    public record Response
    { 
  
        [Required]
        [DataMember(Name="tags")]
        public List<string> Tags { get; set; }
    }
}
