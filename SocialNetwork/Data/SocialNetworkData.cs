using SocialNetwork.Models;

namespace SocialNetwork.Data
{
    public class SocialNetworkData
    {
        public ICollection<User> User { get; set; }

        public SocialNetworkData()
        {
            User = new List<User> { new User("admin") };
        }
    }
}