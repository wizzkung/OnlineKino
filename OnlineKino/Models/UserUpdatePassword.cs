namespace OnlineKino.Models
{
    public class UserUpdatePassword
    {
        public string Login { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public interface IPasswordService
    {
         Task UpdatePasswordAsync(UserUpdatePassword dto);
    }
}
