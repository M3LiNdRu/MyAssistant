using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyAssistant.Apis.Expenses.Api.Resources.Appointments
{
    [DataContract]
    public record Response
    {
        [Required]
        [DataMember(Name = "appointments")]
        public List<Request> Appointments { get; set; }
    }
}
