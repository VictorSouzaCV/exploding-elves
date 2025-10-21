using System;

namespace ExplodingElves.Core
{
    public interface IElfAdapter
    {
        (float x, float y) Position { get; }
        Action<IElfAdapter> OnHitElf { get; set; }
        void ChangeMovement(float x, float y);
        void SetPosition(float x, float y);
        void SetColor((float r, float g, float b, float a) color);
        void ShowStateVisualChange(ElfState state);
    }
}