using Friends.Helpers;
using Friends.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Friends
{
    internal class Person : IPerson
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Mood Mood { get; set; }
        public IEnumerable<string> FriendsIds { get; set; }
        public bool HasParent { get; set; }
        public List<string> PathIds { get; set; }=new List<string>();

        public Person(string id, string name, int age, Mood mood, IEnumerable<string> friendsIds)
        {
            Id = id;
            Name = name;
            Age = age;
            Mood = mood;
            FriendsIds = friendsIds;
        }

        public Person(string id, string name, int age, Mood mood, IEnumerable<string> friendsIds, bool hasParent, List<string> pathIds) 
            : this(id, name, age, mood, friendsIds)
        {
            HasParent = hasParent;
            PathIds = pathIds;
        }

        public Person(string id)
        {
            Id = id;
        }

        public override string ToString()
        {         
            var personToString = new StringBuilder();
            personToString.Append(Id);
            personToString.Append(",");
            personToString.Append(Name);
            personToString.Append(",");
            personToString.Append(Age);
            personToString.Append(",");
            personToString.Append(string.Join(";", Mood.ToString()));
            
            if (FriendsIds.Any())
            { 
                personToString.Append(",");
                personToString.Append(string.Join(";", FriendsIds));
            }
            personToString.Append("|");
            return personToString.ToString();
        }
    }
}
