using System;

namespace ExplodingElves.Core
{
    public class ElfSpawnerDomain : IDisposable
    {
        public Action<IElfAdapter, ElfDomain> OnElfSpawned { get; set; }
        public Action<IElfAdapter> OnElfDespawned { get; set; }
        private readonly IElfSpawnerAdapter _elfSpawnerAdapter;
        private readonly IElfData _elfData;
        private readonly IElfAdapter _elfAdapter;
        private readonly IClockAdapter _clockAdapter;
        private float _lastSpawnTime;
        private float _spawnFrequency;
        private float _startAngularVariation;
        public ElfSpawnerDomain(IElfSpawnerAdapter elfSpawnerAdapter, IElfData elfData, IElfAdapter elfAdapter, IClockAdapter clockAdapter, float startAngularVariation)
        {
            _elfSpawnerAdapter = elfSpawnerAdapter;
            _elfData = elfData;
            _elfAdapter = elfAdapter;
            _spawnFrequency = _elfData.SpawnFrequency;
            _clockAdapter = clockAdapter;
            _startAngularVariation = startAngularVariation;
            _elfSpawnerAdapter.SetColor(_elfData.Color);

            _clockAdapter.OnTick += OnTick;
            _elfSpawnerAdapter.OnSpawnFrequencyChanged += OnSpawnFrequencyChanged;
        }

        private void OnTick(float time)
        {
            if (_clockAdapter.CurrentTime - _lastSpawnTime >= 1f / _spawnFrequency)
            {
                _lastSpawnTime = _clockAdapter.CurrentTime;
                SpawnElf();
            }
        }

        public ElfDomain SpawnElf()
        {
            var elfAdapter = _elfSpawnerAdapter.Spawn(_elfAdapter);
            var elf = new ElfDomain(elfAdapter, _elfData, _clockAdapter, _startAngularVariation);
            elf.OnExplode += OnElfDespawned;
            OnElfSpawned?.Invoke(elfAdapter, elf);
            return elf;
        }

        public ElfDomain SpawnElf(float x, float y)
        {
            var elf = SpawnElf();
            elf.SetPosition(x, y);
            return elf;
        }

        private void OnSpawnFrequencyChanged(float spawnFrequency)
        {
            _spawnFrequency = spawnFrequency;
        }

        public void Dispose()
        {
            _clockAdapter.OnTick -= OnTick;
            _elfSpawnerAdapter.OnSpawnFrequencyChanged -= OnSpawnFrequencyChanged;
        }
    }
}