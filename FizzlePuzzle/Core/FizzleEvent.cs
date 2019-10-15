namespace FizzlePuzzle.Core
{
    public delegate void FizzleEvent();

    public delegate void FizzleEvent<in T>(T value);

    public delegate void FizzleEvent<in T1, in T2>(T1 value1, T2 value2);
}
