using System;

namespace ExplodingElves.Core
{
    public class ElfDomain
    {
        public bool CanBreed 
        {
            get {
                var isOldEnough = _age >= _elfData.ReadyToBreedAge;
                var isCooledDown = _lastBreedTick >= _elfData.BreedCooldown || _lastBreedTick == default;
                return isOldEnough && isCooledDown;
            }
        }
        
        public Action<IElfAdapter, IElfAdapter> OnElvesHit { get; set; }
        public Action<IElfAdapter> OnExplode { get; set; }
        public ElfType ElfType => _elfData.ElfType;
        private readonly IElfAdapter _elfAdapter;
        private readonly IElfData _elfData;
        float _currentAngleVariation;
        float _age => _currentTick - _bornTick;
        float _currentTick;
        float _bornTick;
        float _lastBreedTick;
        Random _random = new Random();
        private readonly IElfAdapter _elfParentAdapter;
        private readonly IElfAdapter _otherParentElfAdapter;

        public ElfDomain(IElfAdapter elfAdapter, IElfData elfData, IElfAdapter parentElfAdapter = null, IElfAdapter otherParentElfAdapter = null)
        {
            _elfAdapter = elfAdapter;
            _elfData = elfData;
            _elfParentAdapter = parentElfAdapter;
            _otherParentElfAdapter = otherParentElfAdapter;

            _elfAdapter.SetColor(_elfData.Color);

            SubscribeToAdapter();

            _currentAngleVariation = (float)(_random.NextDouble() * Math.PI * 2);
        }

        private void OnTick(float time)
        {
            if (_bornTick == default)
            {
                _bornTick = time;
            }

            _currentTick = time;

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
            _lastBreedTick = _elfAdapter.CurrentTime;
        }

        public void Explode()
        {
            UnsubscribeFromAdapter();
            OnExplode?.Invoke(_elfAdapter);
            _elfAdapter.Move(0, 0);
            _elfAdapter.Explode();
            _lastBreedTick = default;
            _bornTick = default;
            _currentTick = default;
        }

        private void SubscribeToAdapter()
        {
            _elfAdapter.OnTick += OnTick;
            _elfAdapter.OnHitElf += OnHitElf;
            _elfAdapter.OnHitWall += OnHitWall;
        }

        private void UnsubscribeFromAdapter()
        {
            _elfAdapter.OnTick -= OnTick;
            _elfAdapter.OnHitElf -= OnHitElf;
            _elfAdapter.OnHitWall -= OnHitWall;
        }
    }
}