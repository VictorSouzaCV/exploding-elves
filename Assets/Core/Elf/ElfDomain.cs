using System;

namespace ExplodingElves.Core
{
    public class ElfDomain
    {
        public bool CanBreed => _isOldEnoughToBreed && !_isTiredOfBreeding;
        public bool CanExplode => _state != ElfState.Exploded;
        public Action<IElfAdapter, IElfAdapter> OnElvesHit { get; set; }
        public Action<IElfAdapter> OnExplode { get; set; }
        public ElfType ElfType => _elfData.ElfType;
        private readonly IElfAdapter _elfAdapter;
        private readonly IElfData _elfData;
        private readonly IClockAdapter _clockAdapter;
        private float _currentAngleVariation;
        private float _currentMovementX;
        private float _currentMovementY;
        private bool _isOldEnoughToBreed => _age >= _elfData.ReadyToBreedAge;
        private bool _isTiredOfBreeding => _lastBreedTime == default ? false : _clockAdapter.CurrentTime - _lastBreedTime <= _elfData.BreedCooldown;
        private float _age => _bornTime == default ? 0 : _clockAdapter.CurrentTime - _bornTime;
        private float _bornTime;
        private float _lastBreedTime;
        private Random _random = new Random();
        private ElfState _state;

        public ElfDomain(IElfAdapter elfAdapter, IElfData elfData, IClockAdapter clockAdapter, float startAngularVariation)
        {
            _elfAdapter = elfAdapter;
            _elfData = elfData;
            _clockAdapter = clockAdapter;

            _elfAdapter.SetColor(_elfData.Color);
            _currentAngleVariation = startAngularVariation;

            ChangeState(ElfState.Minor);
        }

        public void BecomeParent()
        {
            ChangeState(ElfState.TiredOfBreeding);
        }

        public void Explode()
        {
            ChangeState(ElfState.Exploded);
        }

        public void SetPosition(float x, float y)
        {
            _elfAdapter.SetPosition(x, y);
        }

        private void OnUpdate(float deltaTime)
        {
            HandleState();
            CalculateState();
        }

        private void ChangeState(ElfState state)
        {
            _state = state;
            switch (_state)
            {
                case ElfState.Minor:
                    SubscribeToAdapters();
                    _bornTime = _clockAdapter.CurrentTime;
                    break;
                case ElfState.ReadyToBreed:
                    break;
                case ElfState.TiredOfBreeding:
                    _lastBreedTime = _clockAdapter.CurrentTime;
                    break;
                case ElfState.Exploded:
                    _lastBreedTime = 0;
                    _bornTime = 0;
                    _elfAdapter.ChangeMovement(0, 0);
                    UnsubscribeFromAdapters();
                    OnExplode?.Invoke(_elfAdapter);
                    break;
            }
            _elfAdapter.ShowStateVisualChange(_state);
        }

        private void HandleState()
        {
            switch (_state)
            {
                case ElfState.Exploded:
                    break;
                case ElfState.Minor:
                case ElfState.ReadyToBreed:
                case ElfState.TiredOfBreeding:
                    Move();
                    break;
            }
        }

        private void CalculateState()
        {
            switch (_state)
            {
                case ElfState.Minor:
                    if (_isOldEnoughToBreed)
                    {
                        ChangeState(ElfState.ReadyToBreed);
                    }
                    break;
                case ElfState.TiredOfBreeding:
                    if (!_isTiredOfBreeding)
                    {
                        ChangeState(ElfState.ReadyToBreed);
                    }
                    break;
            }
        }

        private void Move()
        {
            float angularVariation = (float)((_random.NextDouble() - 0.5) * _elfData.WanderFactor);
            _currentAngleVariation += angularVariation;

            _currentMovementX = (float)Math.Cos(_currentAngleVariation) * _elfData.Speed;
            _currentMovementY = (float)Math.Sin(_currentAngleVariation) * _elfData.Speed;

            _elfAdapter.ChangeMovement(_currentMovementX, _currentMovementY);
        }

        private void OnHitElf(IElfAdapter other)
        {
            OnElvesHit?.Invoke(_elfAdapter, other);
        }

        private void SubscribeToAdapters()
        {
            _elfAdapter.OnHitElf += OnHitElf;
            _clockAdapter.OnTick += OnUpdate;
        }

        private void UnsubscribeFromAdapters()
        {
            _clockAdapter.OnTick -= OnUpdate;
            _elfAdapter.OnHitElf -= OnHitElf;
        }
    }
}