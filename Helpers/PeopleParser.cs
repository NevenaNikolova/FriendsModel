using Friends.Interfaces;
using Friends.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Friends.Helpers
{
    internal static class PeopleParser
    {
        internal static IPerson ParsePerson(string personAsString)
        {
            //A1,Annie,25,Bored,|J1,John,35,Happy;Calm,M2
            var properties = personAsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var id = properties[0];
            var name = properties[1];
            int.TryParse(properties[2], out int age);
            Mood moodsEnum;
            if (properties.Length <= 3)
            {
                moodsEnum = Mood.None;
            }
            var moodsStr = properties[3].Replace(';', ',');
            var success = Enum.TryParse(moodsStr, out moodsEnum);

            var friendsIds = new List<string>();

            if (properties.Length > 4)
            {
                var ids = properties[4].Split(';');
                foreach (var item in ids)
                {
                    friendsIds.Add(item);
                }
            }
            FriendsModelFactory friendsModelFactory = new FriendsModelFactory();
            return friendsModelFactory.CreatePerson(id, name, age, moodsEnum, friendsIds);
        }
        internal static string ParsePeople(IEnumerable<IPerson> people)
        {
            StringBuilder peopleSB = new StringBuilder();

            foreach (IPerson person in people)
            {               
                peopleSB.Append(person.ToString());
            }

            return peopleSB.ToString();
        }
    }
}
