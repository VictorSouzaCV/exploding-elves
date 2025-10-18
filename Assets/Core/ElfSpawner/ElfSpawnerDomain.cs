using System;
using System.Collections.Generic;

namespace ExplodingElves.Core
{
    public class ElfSpawnerDomain : IDisposable
    {
        private readonly IElfSpawnerAdapter _elfSpawnerAdapter;
        private readonly IElfData _elfData;
        private float _currentTime;
        private float _lastSpawnTime;
        private float _spawnFrequency;
        private List<ElfDomain> _elves = new List<ElfDomain>();
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
            if (_currentTime - _lastSpawnTime >= 1f / _spawnFrequency)
            {
                _lastSpawnTime = _currentTime;
                SpawnElf();
            }
        }

        void SpawnElf()
        {
            var elfAdapter = _elfSpawnerAdapter.Spawn(_elfData.ElfAdapter);
            var elf = new ElfDomain(elfAdapter);
            _elves.Add(elf);
        }

        private void OnSpawnFrequencyChanged(float spawnFrequency)
        {
            _spawnFrequency = spawnFrequency;
        }

        public void Dispose()
        {
            _elfSpawnerAdapter.OnTick -= OnTick;
            _elfSpawnerAdapter.OnSpawnFrequencyChanged -= OnSpawnFrequencyChanged;
            foreach (var elf in _elves)
            {
                elf.Dispose();
            }
        }
    }
}