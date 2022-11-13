using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models
{
    public class User
    {
        public string Login { get; set; }

        [Display(Name = "Creation Date & Time")]
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        public ICollection<User> Friends { get; set; } = new List<User>();

        public User()
        {
        }

        public User(string login)
        {
            Login = login;
        }

        public User(string login, DateTime creationDate)
        {
            Login = login;
            CreationDateTime = creationDate;
        }
    }
}