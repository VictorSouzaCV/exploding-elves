namespace ExplodingElves.Core
{
    public interface IElfData
    {
        ElfType ElfType { get; }
        IElfAdapter ElfAdapter { get; }
        float SpawnFrequency { get; }
    }
}