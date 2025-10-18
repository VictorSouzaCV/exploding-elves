using System;

namespace ExplodingElves.Core
{
    public interface IElfSpawnerAdapter : IClockAdapter
    {
        Action<float> OnSpawnFrequencyChanged { get; set; }
        IElfAdapter Spawn(IElfAdapter elf);
    }
}