using UnityEngine;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    [CreateAssetMenu(fileName = "ElfConfig", menuName = "ExplodingElves/ElfConfig")]
    public class ElfConfig : ScriptableObject, IElfData
    {
        [SerializeField] private Color _color;
        [SerializeField] private ElfType _elfType;
        [SerializeField] private float _spawnFrequency = 1f;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private int _readyToBreedAge = 1;
        [SerializeField] private float _breedCooldown = 1;
        [SerializeField] private float _wanderFactor = 0.3f;
        public (float r, float g, float b, float a) Color => new (_color.r, _color.g, _color.b, _color.a);
        public float SpawnFrequency => _spawnFrequency;
        public ElfType ElfType => _elfType;
        public float Speed => _speed;
        public float ReadyToBreedAge => _readyToBreedAge;
        public float BreedCooldown => _breedCooldown;
        public float WanderFactor => _wanderFactor;
    }
}
