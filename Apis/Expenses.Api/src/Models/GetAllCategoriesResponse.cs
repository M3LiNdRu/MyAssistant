using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MyAssistant.Apis.Expenses.Api.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class GetAllCategoriesResponse : IEquatable<GetAllCategoriesResponse>
    { 
        /// <summary>
        /// Gets or Sets Expenses
        /// </summary>

        [DataMember(Name="expenses")]
        public List<Category> Expenses { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetAllCategoriesResponse {\n");
            sb.Append("  Expenses: ").Append(Expenses).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((GetAllCategoriesResponse)obj);
        }

        /// <summary>
        /// Returns true if GetAllCategoriesResponse instances are equal
        /// </summary>
        /// <param name="other">Instance of GetAllCategoriesResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetAllCategoriesResponse other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Expenses == other.Expenses ||
                    Expenses != null &&
                    Expenses.SequenceEqual(other.Expenses)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Expenses != null)
                    hashCode = hashCode * 59 + Expenses.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(GetAllCategoriesResponse left, GetAllCategoriesResponse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GetAllCategoriesResponse left, GetAllCategoriesResponse right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
