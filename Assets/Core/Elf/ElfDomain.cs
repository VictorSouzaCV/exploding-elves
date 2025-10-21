using System;

namespace ExplodingElves.Core
{
    public class ElfDomain
    {
        public bool CanBreed 
        {
            get {
                var isOldEnough = _age >= _elfData.ReadyToBreedAge;
                var isCooledDown = _lastBreedTime - _clockAdapter.CurrentTime >= _elfData.BreedCooldown || _lastBreedTime == default;
                return isOldEnough && isCooledDown;
            }
        }
        
        public Action<IElfAdapter, IElfAdapter> OnElvesHit { get; set; }
        public Action<IElfAdapter> OnExplode { get; set; }
        public ElfType ElfType => _elfData.ElfType;
        private readonly IElfAdapter _elfAdapter;
        private readonly IElfData _elfData;
        float _currentAngleVariation;
        float _age => _bornTime == default ? 0 : _clockAdapter.CurrentTime - _bornTime;
        float _bornTime;
        float _lastBreedTime;
        Random _random = new Random();
        private readonly IElfAdapter _elfParentAdapter;
        private readonly IElfAdapter _otherParentElfAdapter;
        private readonly IClockAdapter _clockAdapter;
        public ElfDomain(IElfAdapter elfAdapter, IElfData elfData, IClockAdapter clockAdapter, IElfAdapter parentElfAdapter = null, IElfAdapter otherParentElfAdapter = null)
        {
            _elfAdapter = elfAdapter;
            _elfData = elfData;
            _clockAdapter = clockAdapter;
            _elfParentAdapter = parentElfAdapter;
            _otherParentElfAdapter = otherParentElfAdapter;

            _elfAdapter.SetColor(_elfData.Color);
            SubscribeToAdapters();
            _currentAngleVariation = (float)(_random.NextDouble() * Math.PI * 2);
            _bornTime = clockAdapter.CurrentTime;
        }

        private void OnTick(float time)
        {
            float angularVariation = (float)((_random.NextDouble() - 0.5) * 0.3);
            _currentAngleVariation += angularVariation;

            float currentX = (float)Math.Cos(_currentAngleVariation);
            float currentY = (float)Math.Sin(_currentAngleVariation);

            _elfAdapter.Move(currentX * _elfData.Speed, currentY * _elfData.Speed);
        }

        private void OnHitElf(IElfAdapter other)
        {
            if (_elfParentAdapter == other || _otherParentElfAdapter == other)
            {
                return;
            }

            OnElvesHit?.Invoke(_elfAdapter, other);
        }

        private void OnHitWall()
        {
            _currentAngleVariation = (float)(_currentAngleVariation + Math.PI);
        }

        public void BecomeParent()
        {
            _lastBreedTime = _clockAdapter.CurrentTime;
        }

        public void Explode()
        {
            UnsubscribeFromAdapters();
            OnExplode?.Invoke(_elfAdapter);
            _elfAdapter.Move(0, 0);
            _elfAdapter.Explode();
            _lastBreedTime = default;
            _bornTime = default;
        }

        private void SubscribeToAdapters()
        {
            _elfAdapter.OnHitElf += OnHitElf;
            _elfAdapter.OnHitWall += OnHitWall;
            _clockAdapter.OnTick += OnTick;
        }

        private void UnsubscribeFromAdapters()
        {
            _clockAdapter.OnTick -= OnTick;
            _elfAdapter.OnHitElf -= OnHitElf;
            _elfAdapter.OnHitWall -= OnHitWall;
        }
    }
}