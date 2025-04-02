using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace OnlineKino.Models
{
    public class Movies
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Genres { get; set; }
        public int Duration { get; set; }
        public string Link { get; set; }
        public string PosterUrl { get; set; }
        [JsonIgnore]
        [ValidateNever]
         public virtual List<Reviews> Reviews { get; set; }
    }
}
