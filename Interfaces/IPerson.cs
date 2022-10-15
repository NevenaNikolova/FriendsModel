using Friends.Helpers;
using System.Collections.Generic;

namespace Friends.Interfaces
{
    // You need to implement this interface so it reflects its data as parsed from the database model.
    // you can add as many properties and methods if required for your implementation. You just need to make sure
    // you don't break the FriendsModelTester contract
    public interface IPerson
    {
        string Id { get; }
        string Name { get; }
        int Age { get; }
        Mood Mood { get; }      
        IEnumerable<string> FriendsIds { get; }
        bool HasParent { get; set; }
        List<string> PathIds { get; set; }
    }
}