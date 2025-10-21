using System.Collections.Generic;

namespace ExplodingElves.Core
{
    public class ElfHitController
    {
        Dictionary<IElfAdapter, ElfDomain> _elves = new Dictionary<IElfAdapter, ElfDomain>();
        Dictionary<ElfType, ElfSpawnerDomain> _elfSpawnerByType = new Dictionary<ElfType, ElfSpawnerDomain>();

        public void AddElfSpawner(ElfType elfType, ElfSpawnerDomain elfSpawner)
        {
            _elfSpawnerByType[elfType] = elfSpawner;
            elfSpawner.OnElfSpawned += AddElf;
        }

        public void AddElf(IElfAdapter elfAdapter, ElfDomain elfDomain)
        {
            _elves.Add(elfAdapter, elfDomain);
            elfDomain.OnElvesHit += OnElvesHit;
            elfDomain.OnExplode += OnElfExplode;
        }

        private void OnElvesHit(IElfAdapter self, IElfAdapter other)
        {
            ElfDomain elfDomain = null;
            ElfDomain otherElfDomain = null;
            try {
                elfDomain = _elves[self];
                otherElfDomain = _elves[other];
            } catch (KeyNotFoundException) {
            
                return;
            }

            if (elfDomain.ElfType != otherElfDomain.ElfType)
            {
                HandleOppositeElvesHit(elfDomain, otherElfDomain, self, other);
            } 
            else 
            {
                HandleSameElvesHit(elfDomain, otherElfDomain, self, other);
            }
        }

        void HandleOppositeElvesHit(ElfDomain elfDomain, ElfDomain otherElfDomain, IElfAdapter elfAdapter, IElfAdapter otherElfAdapter)
        {
            RemoveElf(elfAdapter);
            RemoveElf(otherElfAdapter);
            elfDomain.Explode();
            otherElfDomain.Explode();
        }

        void HandleSameElvesHit(ElfDomain elfDomain, ElfDomain otherElfDomain, IElfAdapter elfAdapter, IElfAdapter otherElfAdapter)
        {
            if (!elfDomain.CanBreed || !otherElfDomain.CanBreed)
            {
                return;
            }

            elfDomain.BecomeParent();
            otherElfDomain.BecomeParent();
            (float x, float y) newElfPosition = ((elfAdapter.Position.x + otherElfAdapter.Position.x) / 2, (elfAdapter.Position.y + otherElfAdapter.Position.y) / 2);
            _elfSpawnerByType[elfDomain.ElfType].GiveBirthToElf(newElfPosition.x, newElfPosition.y, elfAdapter, otherElfAdapter);
        }

        private void OnElfExplode(IElfAdapter elfAdapter)
        {
            RemoveElf(elfAdapter);
        }

        private void RemoveElf(IElfAdapter elfAdapter)
        {
            if (_elves.ContainsKey(elfAdapter))
            {
                _elves.Remove(elfAdapter);
            }
        }
    }
}