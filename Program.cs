using Friends.Models;

namespace Friends
{
    /*
     * You need to implement the FriendsModel and the FriendsModelFactory such that all the tests in the FriendsModelTester would pass.
     * You need to modify the Mood enum definition - see Mood.cs for details.
     * See IFriendsModel and IModelFactory for details.
     *
     *
     * the sample data (contained in the friendsdb.txt) has the following schema:
     *  id,name,age,mood1;mood2,friendId1;friendId2;friendId3|id,name,age,mood1;mood2,friendId1;friendId2;friendId3|
     *
     *  Person details, separated by pipe symbol (|) where each person details have the following schema (each of the fields is comma separated):
     *      id: string (not containing separators symbols ,;| )
     *      name: string (not containing separators symbols  ,;| )
     *      age: int
     *      mood: 1 or more of [angry, sad, happy, bored, calm] separated by ';' - this has to be represented by enum and has to be case insensitive
     *      friends: array of strings, separated by ';', where each string is an id of a person
     *  
     *  NOTE: assume friendship database file is always valid and does not contain malformed input and/or missing person fields
     *  NOTE: friendship is not bidirectional, so John can be a friend of Mary, while Mary is not a friend of John
     *
     *
     *
     *
     *  You can see the pictures for further clarification:
     *      FriendsGraph.png - visualizes the graph from the sample database
     *      MaryFriendsGraph.png - visualizes the friends bubble graph from the perspective of Marry - it shows immediate friends and their friends etc.
     */

    class Program
    {
        static void Main(string[] args)
        {
            var model = new FriendsModel();
            model.Rebuild(Constants.ImportFilePath);
            var friendsModelFactory = new FriendsModelFactory();
            new FriendsModelTester(model, friendsModelFactory, "friendsdb.txt").Test();
        }
    }
}