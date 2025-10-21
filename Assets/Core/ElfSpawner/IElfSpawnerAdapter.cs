using System;

namespace ExplodingElves.Core
{
    public interface IElfSpawnerAdapter
    {
        Action<float> OnSpawnFrequencyChanged { get; set; }
        IElfAdapter Spawn(IElfAdapter elf);
        IElfAdapter Spawn(IElfAdapter elf, float x, float y);
    }
}