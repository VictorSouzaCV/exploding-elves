using System;

namespace ExplodingElves.Core
{
    public class ElfDomain : IDisposable
    {
        private readonly IElfAdapter _elfAdapter;
        float _currentX;
        float _currentY;

        public ElfDomain(IElfAdapter elfAdapter)
        {
            _elfAdapter = elfAdapter;
            _elfAdapter.OnTick += OnTick;
        }

        private void OnTick(float time)
        {
            _elfAdapter.Move(_currentX, _currentY);
        }

        public void Dispose()
        {
            _elfAdapter.OnTick -= OnTick;
        }
    }
}