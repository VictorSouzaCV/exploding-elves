using System;

namespace ExplodingElves.Core
{
    public interface IElfAdapter : IClockAdapter
    {
        Action<IElfAdapter> OnHitElf { get; set; }
        void Move(float x, float y);
    }
}