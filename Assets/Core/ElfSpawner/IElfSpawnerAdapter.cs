using System;

namespace ExplodingElves.Core
{
    public interface IElfSpawnerAdapter : IClockAdapter
    {
        Action<float> OnSpawnFrequencyChanged { get; set; }
        void Spawn(IElfAdapter elf);
    }
}