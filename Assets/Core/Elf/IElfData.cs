namespace ExplodingElves.Core
{
    public interface IElfData
    {
        IElfAdapter ElfAdapter { get; }
        float SpawnFrequency { get; }
    }
}