using System.Collections.Generic;

namespace ExplodingElves.Core
{
    public class ElfHitDomain
    {
        Dictionary<IElfAdapter, ElfDomain> _elves = new Dictionary<IElfAdapter, ElfDomain>();
        Dictionary<ElfType, ElfSpawnerDomain> _elfSpawnerByType = new Dictionary<ElfType, ElfSpawnerDomain>();

        public void AddElfSpawner(ElfType elfType, ElfSpawnerDomain elfSpawner)
        {
            _elfSpawnerByType[elfType] = elfSpawner;
            elfSpawner.OnElfSpawned += AddElf;
        }

        private void AddElf(IElfAdapter elfAdapter, ElfDomain elfDomain)
        {
            _elves.Add(elfAdapter, elfDomain);
            elfDomain.OnElvesHit += OnElvesHit;
        }

        private void RemoveElf(IElfAdapter elfAdapter)
        {
            var elfDomain = _elves[elfAdapter];
            elfDomain.OnElvesHit -= OnElvesHit;
            _elves.Remove(elfAdapter);
        }

        private void OnElvesHit(IElfAdapter self, IElfAdapter other)
        {
            ElfDomain elfDomain;
            ElfDomain otherElfDomain;
            try
            {
                elfDomain = _elves[self];
                otherElfDomain = _elves[other];
            }
            catch (KeyNotFoundException)
            {
                //Expected to happen when elves are exploding
                return;
            }
            if (elfDomain.ElfType != otherElfDomain.ElfType && elfDomain.CanExplode && otherElfDomain.CanExplode)
            {
                ExplodeElves(elfDomain, otherElfDomain, self, other);
            }
            else if (elfDomain.CanBreed && otherElfDomain.CanBreed)
            {
                CreateElvesFamily(elfDomain, otherElfDomain, self, other);
            }
        }

        void ExplodeElves(ElfDomain elfDomain, ElfDomain otherElfDomain, IElfAdapter elfAdapter, IElfAdapter otherElfAdapter)
        {
            elfDomain.Explode();
            otherElfDomain.Explode();
            RemoveElf(elfAdapter);
            RemoveElf(otherElfAdapter);
        }

        void CreateElvesFamily(ElfDomain elfDomain, ElfDomain otherElfDomain, IElfAdapter elfAdapter, IElfAdapter otherElfAdapter)
        {
            elfDomain.BecomeParent();
            otherElfDomain.BecomeParent();
            (float x, float y) newElfPosition = ((elfAdapter.Position.x + otherElfAdapter.Position.x) / 2, (elfAdapter.Position.y + otherElfAdapter.Position.y) / 2);
            _elfSpawnerByType[elfDomain.ElfType].SpawnElf(newElfPosition.x, newElfPosition.y);
        }
    }
}