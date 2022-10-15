using Friends.Helpers;
using Friends.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Friends.Models
{
    // your solution implementation should go in here
    // you can create as much classes and types (if you need to)

    // your main implementation can go in here
    internal class FriendsModel : IFriendsModel
    {
        private List<IPerson> people = new List<IPerson>();
        public void Rebuild(string databaseFilePath)
        {
            var input = DataHandler.Read(databaseFilePath);
            if (input == null)
            {
                throw new ArgumentNullException(Constants.InvalidInput);
            }
            var splitInput = input.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var people = new List<Person>();

            foreach (var p in splitInput)
            {
                var person = PeopleParser.ParsePerson(p) as Person;
                people.Add(person);
            }
            FillPeopleList(people);
        }
        public void Export(string databaseFilePath)
        {
            var people = GetPeople().ToList();
            var content = PeopleParser.ParsePeople(people);
            DataHandler.Write(content, databaseFilePath);
        }

        public void AddPerson(IPerson p)
        {
            var currentList = GetPeople().ToList();
            if (currentList.Any(x => x.Id == p.Id))
            {
                throw new InvalidOperationException(Constants.IdAlreadyExists);
            }
            currentList.Add(p);
            FillPeopleList(currentList);                    
        }

        public IPerson GetPerson(string id)
        {
            return GetPeople().FirstOrDefault(p => p.Id == id);
        }

        public bool RemovePerson(IPerson p)
        {
            List<IPerson> newList = GetPeople().ToList();
            if (!newList.Any(x => x.Id == p.Id))
            {
                return false;
            }
            newList.Remove(p);
            FillPeopleList(newList);
            return true;
        }

        public IEnumerable<IPerson> GetPeople()
        {
            return people;
        }
        //BFS implemented with queue
        public IEnumerable<IPerson> GetFriendsInDepth(string personId)
        {
            var result = new List<IPerson>();
            var enqueuedIds = new List<string>();

            Queue<IPerson> visitedPeopleQueue = new Queue<IPerson>();
            var person = GetPerson(personId);

            visitedPeopleQueue.Enqueue(person);

            while (visitedPeopleQueue.Count > 0)
            {
                var currentPerson = visitedPeopleQueue.Dequeue();
                if (currentPerson==null)
                {
                    continue;
                }
                result.Add(currentPerson);
                enqueuedIds.Add(currentPerson.Id);

                var friendsIds = currentPerson.FriendsIds;

                foreach (var id in friendsIds)
                {
                    if (enqueuedIds.IndexOf(id) != -1)
                    {
                        continue;
                    }
                    visitedPeopleQueue.Enqueue(GetPerson(id));
                }
            }
            //Removes the start person from the result
            result.RemoveAt(0);
            return result;
        }

        //DFS implemented using stack
        public IEnumerable<Tuple<IPerson, IEnumerable<string>>> GetSocialBubble(string personId,
            Func<IPerson, bool> filter = null, int maxDepthLevel = -1)
        {
            List<Tuple<IPerson, IEnumerable<string>>> result = new List<Tuple<IPerson, IEnumerable<string>>>();
            var startPerson = GetPerson(personId);
            startPerson.PathIds.Clear();
            startPerson.PathIds.Add(string.Empty);
            var currentDepthLevel = -1;
            var visited = new HashSet<string>();
            //Keeps a record of the already added ids to avoid adding them twice
            var inStack = new HashSet<string>();
            
            var stack = new Stack<IPerson>();
            stack.Push(startPerson);

            while (stack.Count != 0)
            {
                var currentPerson = stack.Pop();
                if (!visited.Contains(currentPerson.Id))
                {
                    visited.Add(currentPerson.Id);
                }
                //Removes the empty string added from the start person
                var bubble = currentPerson.PathIds;
                if (bubble.Count > 1 && bubble[0] == String.Empty)
                {
                    bubble.RemoveAt(0);
                }
                result.Add(new Tuple<IPerson, IEnumerable<string>>(currentPerson, bubble));
                var friendsIds = currentPerson.FriendsIds.Where(x => !visited.Contains(x));
                //Using Math.Abs to get always a positive int.
                //Not sure why the default is -1, since readme.md shows the depths in positive numbers
                if (currentDepthLevel <= Math.Abs(maxDepthLevel))
                {
                    foreach (var id in friendsIds)
                    {
                        if (inStack.Contains(id))
                        {
                            continue;
                        }
                        inStack.Add(id);
                        var person = GetPerson(id) as Person;
                        person.HasParent = true;

                        if (currentDepthLevel == 0)
                        {
                            person.PathIds.Add(string.Empty);

                        }
                        else
                        {
                            person.PathIds.AddRange(currentPerson.PathIds);
                            person.PathIds.Add(currentPerson.Id);
                        }
                        stack.Push(person);
                    }
                }
                currentDepthLevel++;
            }
            result.RemoveAt(0);
            return result;
        }
        private void FillPeopleList(IEnumerable<IPerson> people)
        {
            this.people = people.ToList();
        }
    }
}