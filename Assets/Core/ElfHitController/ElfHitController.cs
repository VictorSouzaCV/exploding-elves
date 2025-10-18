using System.Collections.Generic;

namespace ExplodingElves.Core
{
    public class ElfHitController
    {
        Dictionary<IElfAdapter, ElfDomain> _elves = new Dictionary<IElfAdapter, ElfDomain>();

        public void AddElf(IElfAdapter elfAdapter, ElfDomain elfDomain)
        {
            _elves.Add(elfAdapter, elfDomain);
            elfDomain.OnElvesHit += OnElvesHit;
            elfDomain.OnExplode += OnElfExplode;
        }

        private void OnElvesHit(IElfAdapter self, IElfAdapter other)
        {
            var elfDomain = _elves[self];
            var otherElfDomain = _elves[other];

            if (elfDomain.ElfType != otherElfDomain.ElfType)
            {
                RemoveElf(self);
                RemoveElf(other);
                elfDomain.Explode();
                otherElfDomain.Explode();
            }
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