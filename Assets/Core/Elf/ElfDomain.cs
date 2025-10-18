using System;

namespace ExplodingElves.Core
{
    public class ElfDomain : IDisposable
    {
        private readonly IElfAdapter _elfAdapter;
        float _currentX;
        float _currentY;
        Random _random = new Random();

        public ElfDomain(IElfAdapter elfAdapter)
        {
            _elfAdapter = elfAdapter;
            _elfAdapter.OnTick += OnTick;
        
            _currentX = _random.Next(-10, 10);
            _currentY = _random.Next(-10, 10);
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