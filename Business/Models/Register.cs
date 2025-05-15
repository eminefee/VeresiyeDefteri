using System.Reflection.Metadata.Ecma335;

namespace Business.Models
{
    public class Register
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
