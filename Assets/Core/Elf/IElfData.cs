namespace ExplodingElves.Core
{
    public interface IElfData
    {
        (float r, float g, float b, float a) Color { get; }
        ElfType ElfType { get; }
        float SpawnFrequency { get; }
        float Speed { get; }
        float ReadyToBreedAge { get; }
        float BreedCooldown { get; }
    }
}