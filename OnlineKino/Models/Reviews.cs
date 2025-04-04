using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineKino.Models
{
    public class Reviews
    {
        
        public int id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey("UserId")]
        [JsonIgnore]
        [ValidateNever]
        public virtual Users? Users { get; set; }
        [ForeignKey("MovieId")]
        [JsonIgnore]
        [ValidateNever]
        public virtual Movies? Movies { get; set; }
    }
}
