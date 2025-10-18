using System;

namespace ExplodingElves.Core
{
    public class ElfSpawnerDomain : IDisposable
    {
        private readonly IElfSpawnerAdapter _elfSpawnerAdapter;
        private readonly IElfData _elfData;
        private float _currentTime;
        private float _lastSpawnTime;
        private float _spawnFrequency;
        public ElfSpawnerDomain(IElfSpawnerAdapter elfSpawnerAdapter, IElfData elfData)
        {
            _elfSpawnerAdapter = elfSpawnerAdapter;
            _elfData = elfData;
            _spawnFrequency = _elfData.SpawnFrequency;

            _elfSpawnerAdapter.OnTick += OnTick;
            _elfSpawnerAdapter.OnSpawnFrequencyChanged += OnSpawnFrequencyChanged;
        }

        private void OnTick(float time)
        {
            _currentTime = time;
            if (_currentTime - _lastSpawnTime >= 1f/_spawnFrequency)
            {
                _lastSpawnTime = _currentTime;
                _elfSpawnerAdapter.Spawn(_elfData.ElfAdapter);
            }
        }

        private void OnSpawnFrequencyChanged(float spawnFrequency)
        {
            _spawnFrequency = spawnFrequency;
        }

        public void Dispose()
        {
            _elfSpawnerAdapter.OnTick -= OnTick;
            _elfSpawnerAdapter.OnSpawnFrequencyChanged -= OnSpawnFrequencyChanged;
        }
    }
}