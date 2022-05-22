using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{ 

    [DataContract]
    public record Request
    { 

        [DataMember(Name="timestamp")]
        public DateTime? Timestamp { get; set; }

        [Required]

        [DataMember(Name="category")]
        public string Category { get; set; }

 
        [DataMember(Name="name")]
        public string Name { get; set; }


        [Required]

        [DataMember(Name="amount")]
        public float Amount { get; set; }


        [DataMember(Name="currency")]
        public string Currency { get; set; }

    }
}
