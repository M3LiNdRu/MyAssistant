using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{ 

    [DataContract]
    public record Response
    { 
  
        [Required]

        [DataMember(Name="expenses")]
        public List<Request> Expenses { get; set; }
    }
}
