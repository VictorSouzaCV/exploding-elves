namespace ExplodingElves.Core
{
    public interface IElfData
    {
        (float r, float g, float b, float a) Color { get; }
        ElfType ElfType { get; }
        IElfAdapter ElfAdapter { get; }
        float SpawnFrequency { get; }
    }
}