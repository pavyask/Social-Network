using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SocialNetwork.Models
{
    public class User
    {
        [Key]
        public string Login { get; set; }

        [Display(Name = "Creation Date & Time")]
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        public virtual ICollection<UserFriendRelation> Friends { get; set; } = new List<UserFriendRelation>();
        public virtual ICollection<UserFriendRelation> FriendOf { get; set; } = new List<UserFriendRelation>();

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

        public void AddFriend(User friend)
        {
            var friendRelation = new UserFriendRelation(this, friend);
            Friends.Add(friendRelation);
        }

        public void RemoveFriend(User friend)
        {
            Friends.Remove(Friends.FirstOrDefault(f => f.FriendLogin == friend.Login));
        }

        public User? GetFriendByLogin(string login) => Friends.FirstOrDefault(f => f.FriendLogin == login).Friend;

        public void RemoveAllFriends()
        {
            Friends.Clear();
        }

        public ICollection<string> GetFriendLogins() => Friends.Select(friend => friend.FriendLogin).ToList();

        public bool IsUserFriend(User user) => Friends.Any(f => f.FriendLogin == user.Login);


    }
}