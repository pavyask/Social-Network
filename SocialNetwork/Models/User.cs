using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class User
    {
        [Key]
        public string Login { get; private set; }

        public DateTime CreationDateTime { get; private set; }

        public IEnumerable<User> Friends { get; private set; }


        public User(string login)
        {
            Login = login;
            CreationDateTime = DateTime.Now;
            Friends = new List<User>();
        }
    }
}