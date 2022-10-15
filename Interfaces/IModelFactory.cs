using Friends.Helpers;
using System.Collections.Generic;

namespace Friends.Interfaces
{
    public interface IModelFactory
    {
        /// <summary>
        ///     Creates a person with the specified fields.
        /// </summary>
        IPerson CreatePerson(string id, string name, int age, Mood mood, IEnumerable<string> friendIds);
    }
}