namespace OnlineKino.Models
{
    public class TokenResultDTO
    {
        public int Status { get; set; }
        public string? UserType { get; set; }
        public int? Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}
