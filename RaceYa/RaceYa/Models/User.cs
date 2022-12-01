

using RaceYa.Helpers;

namespace RaceYa.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }

        public User(string name, string userId)
        {
            Name = name;
            UserId = userId;
            FirestoreUser.Insert(this);
        }
    }
}
