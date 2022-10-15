using System;

namespace Friends.Helpers
{
    //Modify the enum Mood definition so that it supports multiple values
    [Flags]
    public enum Mood
    {
        None = 1,
        Angry = 2,
        Sad = 4,
        Happy = 8,
        Bored = 16,
        Calm = 32
    }
}