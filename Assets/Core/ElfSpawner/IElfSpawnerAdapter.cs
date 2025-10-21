using System;

namespace ExplodingElves.Core
{
    public interface IElfSpawnerAdapter
    {
        Action<float> OnSpawnFrequencyChanged { get; set; }
        IElfAdapter Spawn(IElfAdapter elf);
        void SetColor((float r, float g, float b, float a) color);
    }
}