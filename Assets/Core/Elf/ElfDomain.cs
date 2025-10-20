using System;

namespace ExplodingElves.Core
{
    public class ElfDomain
    {
        public Action<IElfAdapter, IElfAdapter> OnElvesHit { get; set; }
        public Action<IElfAdapter> OnExplode { get; set; }
        public ElfType ElfType => _elfData.ElfType;
        private readonly IElfAdapter _elfAdapter;
        private readonly IElfData _elfData;
        float _currentX;
        float _currentY;
        Random _random = new Random();

        public ElfDomain(IElfAdapter elfAdapter, IElfData elfData)
        {
            _elfAdapter = elfAdapter;
            _elfData = elfData;

            _elfAdapter.SetColor(_elfData.Color);

            SubscribeToAdapter();
        
            _currentX = _random.Next(-30, 30);
            _currentY = _random.Next(-30, 30);
        }

        private void OnTick(float time)
        {
            _elfAdapter.Move(_currentX, _currentY);
        }

        private void OnHitElf(IElfAdapter other)
        {
            OnElvesHit?.Invoke(_elfAdapter, other);
        }

        public void Explode()
        {
            UnsubscribeFromAdapter();
            OnExplode?.Invoke(_elfAdapter);
            _elfAdapter.Explode();
        }

        private void SubscribeToAdapter()
        {
            _elfAdapter.OnTick += OnTick;
            _elfAdapter.OnHitElf += OnHitElf;
        }

        private void UnsubscribeFromAdapter()
        {
            _elfAdapter.OnTick -= OnTick;
            _elfAdapter.OnHitElf -= OnHitElf;
        }
    }
}