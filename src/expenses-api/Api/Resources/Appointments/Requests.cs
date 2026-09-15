using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyAssistant.Apis.Expenses.Api.Resources.Appointments
{
    [DataContract]
    public record Request
    {
        [Required]
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [Required]
        [DataMember(Name = "dateTime")]
        public DateTime DateTime { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime? Timestamp { get; set; }
    }
}
