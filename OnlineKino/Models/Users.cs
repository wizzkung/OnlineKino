namespace OnlineKino.Models
{
    public class Users
    {
        public int id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<Reviews> Reviews { get; set; }
    }

}
