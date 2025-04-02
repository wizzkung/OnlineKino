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
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public virtual Users Users { get; set; }
        [JsonIgnore]
        public virtual Movies Movies { get; set; }
    }
}
