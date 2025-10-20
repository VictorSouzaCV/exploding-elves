using System;

namespace ExplodingElves.Core
{
    public interface IElfAdapter : IClockAdapter
    {
        (float x, float y) Position { get; }
        Action OnHitWall { get; set; }
        Action<IElfAdapter> OnHitElf { get; set; }
        void Move(float x, float y);
        void Explode();
        void SetColor((float r, float g, float b, float a) color);
    }
}