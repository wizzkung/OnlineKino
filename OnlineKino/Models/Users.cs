using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace OnlineKino.Models
{
    public class Users
    {
        public int id { get; set; }
        public string? Email { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
       
        [ValidateNever]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual List<Reviews> Reviews { get; set; }
    }

}
