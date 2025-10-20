using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    [CreateAssetMenu(fileName = "ElfConfig", menuName = "ExplodingElves/ElfConfig")]
    public class ElfConfig : ScriptableObject, IElfData
    {
        [SerializeField] private Color _color;
        [SerializeField] private ElfType _elfType;
        [SerializeField] private ElfBehaviour _elfBehaviour;
        [SerializeField] private float _spawnFrequency = 1f;

        public (float r, float g, float b, float a) Color => new (_color.r, _color.g, _color.b, _color.a);
        public IElfAdapter ElfAdapter => _elfBehaviour;
        public float SpawnFrequency => _spawnFrequency;
        public ElfType ElfType => _elfType;
    }
}
