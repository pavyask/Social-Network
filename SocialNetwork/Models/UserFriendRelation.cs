namespace SocialNetwork.Models
{
    public class UserFriendRelation
    {
        public string UserLogin { get; set; }
        public virtual User User { get; set; }

        public string FriendLogin { get; set; }
        public virtual User Friend { get; set; }

        public UserFriendRelation() { }

        public UserFriendRelation(User user, User friend)
        {
            UserLogin = user.Login;
            User = user;
            FriendLogin = friend.Login;
            Friend = friend;
        }
    }
}
