using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class User
    {
        [Key]
        public string Login { get; }

        public DateTime CreationDateTime { get; }

        public IEnumerable<User> Friends { get; }


        public User(string login)
        {
            Login = login;
            CreationDateTime = DateTime.Now;
            Friends = new List<User>();
        }
    }
}