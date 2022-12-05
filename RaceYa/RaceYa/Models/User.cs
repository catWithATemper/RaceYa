

using RaceYa.Helpers;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace RaceYa.Models
{
    public class User
    {
        [Id]
        public string Id { get; set; }

        [MapTo("userId")]
        public string UserId { get; set; }

        [MapTo("name")]
        public string Name { get; set; }

        public User(string name, string userId)
        {
            Name = name;
            UserId = userId;
            FirestoreUser.Add(this);
        }
    }
}
