using System;

namespace ExplodingElves.Core
{
    public interface IClockAdapter
    {
        Action<float> OnTick { get; set; }
        float CurrentTime { get; }
    }
}