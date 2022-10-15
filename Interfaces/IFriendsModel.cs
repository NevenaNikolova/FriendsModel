using System;
using System.Collections.Generic;

namespace Friends.Interfaces
{
    public interface IFriendsModel
    {
        // resets the current model and rebuilds from the file given by databaseFilePath
        void Rebuild(string databaseFilePath);

        // exports the current model in a new file - specified by databaseFilePath
        void Export(string databaseFilePath);

        // add p only if missing from model (uniqueness checked by p.Id)
        void AddPerson(IPerson p);

        // retrieves person by id, null if missing
        IPerson GetPerson(string id);

        // remove p from the model by its id, true if successfully removed, otherwise false
        bool RemovePerson(IPerson p);

        //returns an enumerable of the current people in the model
        IEnumerable<IPerson> GetPeople();

        // returns friends (without the person itself) of the person specified by its id - this should include all friends (friends of friends) - so transitive
        // ideally this should return the closest friends first
        // note that there could be cycles in the graph - as shown in FriendsGraph.png
        IEnumerable<IPerson> GetFriendsInDepth(string personId);

        // this should return all friends of the specified person (by personId), closest friends first, with the path of names to reach this person,
        // within the specified maxDepthLevel
        //       [John]
        //       /      \
        //    [Mary]   [Peter]
        //     /
        // [Steven]

        // the social bubble of John would be: Mary (empty path) and Peter (empty path), Steven (Mary) 
        // the empty paths are when the friend is an immediate connection

        // the social bubble of Peter would be:
        // John (), Mary(John), Steven(John, Mary)

        // The maxDepthLevel if specified (>= 0) should restrict how deep the bubble should be.
        // so the social bubble of John with max depth 0 - is empty

        // The filter should simply filter out the people but still traverse their friends
        // so the social bubble of Peter with John filtered out is: Mary (John), Steven (John, Mary)
        // so this should be simply the full social bubble without the filtered people

        // note that there could be cycles in the graph - as shown in FriendsGraph.png
        IEnumerable<Tuple<IPerson, IEnumerable<string>>>
            GetSocialBubble(string personId, Func<IPerson, bool> filter = null,
                int maxDepthLevel =
                    -1); // return all friends of p and all the friends of each of the friends of p etc. filtered by the function filter and unique by id
    }
}