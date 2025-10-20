using System;
using System.Collections.Generic;

namespace ExplodingElves.Core
{
    public class ElfSpawnerDomain : IDisposable
    {
        public Action<IElfAdapter, ElfDomain> OnElfSpawned { get; set; }
        public Action<IElfAdapter> OnElfDespawned { get; set; }
        private readonly IElfSpawnerAdapter _elfSpawnerAdapter;
        private readonly IElfData _elfData;
        private readonly IElfAdapter _elfAdapter;
        private float _currentTime;
        private float _lastSpawnTime;
        private float _spawnFrequency;

        public ElfSpawnerDomain(IElfSpawnerAdapter elfSpawnerAdapter, IElfData elfData, IElfAdapter elfAdapter)
        {
            _elfSpawnerAdapter = elfSpawnerAdapter;
            _elfData = elfData;
            _elfAdapter = elfAdapter;
            _spawnFrequency = _elfData.SpawnFrequency;

            _elfSpawnerAdapter.OnTick += OnTick;
            _elfSpawnerAdapter.OnSpawnFrequencyChanged += OnSpawnFrequencyChanged;
        }

        private void OnTick(float time)
        {
            _currentTime = time;
            if (_currentTime - _lastSpawnTime >= 1f / _spawnFrequency)
            {
                _lastSpawnTime = _currentTime;
                SpawnElf();
            }
        }

        public void SpawnElf()
        {
            var elfAdapter = _elfSpawnerAdapter.Spawn(_elfAdapter);
            var elf = new ElfDomain(elfAdapter, _elfData);
            elf.OnExplode += OnElfDespawned;
            OnElfSpawned?.Invoke(elfAdapter, elf);
        }

        public void GiveBirthToElf(float x, float y, IElfAdapter elfParentAdapter, IElfAdapter otherElfParentAdapter)
        {
            var elfAdapter = _elfSpawnerAdapter.Spawn(_elfAdapter, x, y);
            var elf = new ElfDomain(elfAdapter, _elfData, elfParentAdapter, otherElfParentAdapter);
            elf.OnExplode += OnElfDespawned;
            OnElfSpawned?.Invoke(elfAdapter, elf);
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