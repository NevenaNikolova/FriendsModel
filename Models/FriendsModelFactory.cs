using Friends.Helpers;
using Friends.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friends.Models
{
    internal class FriendsModelFactory : IModelFactory
    {
        public IPerson CreatePerson(string id, string name, int age, Mood mood, IEnumerable<string> friendIds)
        {
            return new Person(id, name, age, mood, friendIds);
        }
    }
}
