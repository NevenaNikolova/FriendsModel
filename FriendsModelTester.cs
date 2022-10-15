using Friends.Helpers;
using Friends.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Friends
{
    public class FriendsModelTester
    {
        private class TestAttribute : Attribute
        {
        }

        private readonly string _entryFileLocation;
        private readonly IModelFactory _modelFactory;
        private readonly IFriendsModel _modelUnderTest;

        public FriendsModelTester(IFriendsModel modelUnderTest, IModelFactory modelFactory, string entryFileLocation)
        {
            _modelUnderTest = modelUnderTest;
            _modelFactory = modelFactory;
            _entryFileLocation = entryFileLocation;
        }

        // To avoid adding packages to the solution and at the same time to provide an easy way for testing we have the "Test" method to evaluate the implementation
        public void Test()
        {
            int tests = 0;
            foreach (MethodInfo methodInfo in typeof(FriendsModelTester)
                         .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(m =>
                             m.GetCustomAttribute<TestAttribute>() != null))
            {
                Console.WriteLine($"Running test {methodInfo.Name}...");
                methodInfo.Invoke(this, Array.Empty<object>());
                Console.WriteLine($"Test {methodInfo.Name} PASSED\n");
                tests++;
            }


            Console.WriteLine($"Success. All {tests} tests PASSED");
        }

        [Test]
        private void TestNumberOfPeople()
        {
            _modelUnderTest.Rebuild(_entryFileLocation);
            var resultPeople = _modelUnderTest.GetPeople();

            Debug.Assert(resultPeople.Count() == 5);
        }

        [Test]
        private void TestExportedModel()
        {
            var chain = _modelUnderTest.GetPeople().First(p => p.Name == "Annie");
            for (int i = 0; i < 2500; i++)
            {
                var id = $"af{i}";
                _modelUnderTest.AddPerson(chain =
                    _modelFactory.CreatePerson(id, $"AF{i}", i, Mood.Calm, new List<string> { chain.Id }));
            }

            _modelUnderTest.Export("testout.txt");
            _modelUnderTest.Rebuild("testout.txt");

            var person = _modelUnderTest.GetPerson("af2499");
            Debug.Assert(person != null);
            var socialBubble = _modelUnderTest.GetSocialBubble(person.Id, p => p.Id == "A1").ToList();
            Debug.Assert(socialBubble.Count == 1);
            
            var path = socialBubble[0].Item2.ToList();
            Debug.Assert(path.Count == 2499);
            //TO DO
            //for (int i = 0; i < 2499; i++)
            //{
            //    Debug.Assert(path[i] == _modelUnderTest.GetPerson($"af{2498 - i}").Name);
            //}
        }

        [Test]
        private void TestJohnsDetails()
        {
            var johnId = "J1";

            _modelUnderTest.Rebuild(_entryFileLocation);

            var john = _modelUnderTest.GetPerson(johnId);

			Debug.Assert((john.Mood & Mood.None) == Mood.None);
            Debug.Assert((john.Mood & Mood.Bored) == Mood.None);
            Debug.Assert((john.Mood & Mood.Angry) == Mood.None);
            Debug.Assert((john.Mood & Mood.Sad) == Mood.None);
            
            Debug.Assert((john.Mood & Mood.Happy) == Mood.Happy);
            Debug.Assert((john.Mood & Mood.Calm) == Mood.Calm);
            Debug.Assert(john.Age == 35);        }

        [Test]
        private void TestDepthLevel()
        {
            var johnId = "J1";

            _modelUnderTest.Rebuild(_entryFileLocation);

            var socialBubbleResultZero = _modelUnderTest.GetSocialBubble(johnId, p => true, 0).ToArray();
            Debug.Assert(socialBubbleResultZero.Length == 0);

            var socialBubbleResultOne = _modelUnderTest.GetSocialBubble(johnId, p => true, 1).ToArray();

            Debug.Assert(socialBubbleResultOne.Count() == 1);
            Debug.Assert(socialBubbleResultOne.First().Item1.Name == "Mary");

            var socialBubbleResultTwo = _modelUnderTest.GetSocialBubble(johnId, p => true, 2).ToArray();

            Debug.Assert(socialBubbleResultTwo.Count() == 2);
            Debug.Assert(socialBubbleResultTwo.Any(item => item.Item1.Name == "Jack"));

            var socialBubbleResultThree = _modelUnderTest.GetSocialBubble(johnId, p => true, 3).ToArray();

            Debug.Assert(socialBubbleResultThree.Count() == 3);
            Debug.Assert(socialBubbleResultThree.Any(item => item.Item1.Name == "Peter"));

            var socialBubbleResultFour = _modelUnderTest.GetSocialBubble(johnId, p => true, 4).ToArray();

            Debug.Assert(socialBubbleResultFour.Count() == 4);
            Debug.Assert(socialBubbleResultFour.Any(item => item.Item1.Name == "Annie"));
        }

        [Test]
        private void TestRemovePerson()
        {
            var annieId = "A1";
            var peterId = "P1";
            var maryId = "M2";
            var johnId = "J1";
            var jackId = "J2";
            var randomId = "randomId";
            _modelUnderTest.Rebuild(_entryFileLocation);
            var invalidRemoveResult = _modelUnderTest.RemovePerson(this._modelFactory.CreatePerson(randomId, "Tony", 42, Mood.None, new List<string>()));
            Debug.Assert(!invalidRemoveResult);
            var correctRemoveResult = _modelUnderTest.RemovePerson(_modelUnderTest.GetPerson(annieId));
            Debug.Assert(correctRemoveResult);
            var peopleAfterRemove = _modelUnderTest.GetPeople().ToArray();
            Debug.Assert(peopleAfterRemove.Length == 4);
            var peterFriends = _modelUnderTest.GetFriendsInDepth(peterId).ToArray();
            Debug.Assert(peterFriends.Length == 3);

            Debug.Assert(peterFriends.Any(p => p.Id == maryId));
            Debug.Assert(peterFriends.Any(p => p.Id == johnId));
            Debug.Assert(peterFriends.Any(p => p.Id == jackId));
        }

        [Test]
        private void TestSocialBubblePath()
        {
            var jackId = "J2";
            var johnId = "J1";
            var annieId = "A1";
            var peterId = "P1";

            _modelUnderTest.Rebuild(_entryFileLocation);
            var jackSocialBubble = _modelUnderTest.GetSocialBubble(jackId, _ => true);
            var johnSocialBubble = _modelUnderTest.GetSocialBubble(johnId, _ => true);

            var anniesEntry = jackSocialBubble.Single(entry => entry.Item1.Id == annieId);
            var petersEntry = johnSocialBubble.Single(entry => entry.Item1.Id == peterId);

            var annieFriendsPath = anniesEntry.Item2.ToArray();
            var petersFriendspath = petersEntry.Item2.ToArray();

            var isAnniePathCorrect = annieFriendsPath.SequenceEqual(new[] { "Peter", });
            var isPeterPathCorrect = petersFriendspath.SequenceEqual(new[] { "Mary", "Jack", });

            Debug.Assert(isAnniePathCorrect);
            Debug.Assert(isPeterPathCorrect);
        }
    }
}