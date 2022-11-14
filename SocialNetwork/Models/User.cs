using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SocialNetwork.Models
{
    public class User
    {
        public string Login { get; set; }

        [Display(Name = "Creation Date & Time")]
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        [JsonIgnore]
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