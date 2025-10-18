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
        [SerializeField] private ElfBehaviour _elfBehaviour;
        [SerializeField] private float _spawnFrequency = 1f;

        public IElfAdapter ElfAdapter => _elfBehaviour;
        public float SpawnFrequency => _spawnFrequency;
    }
}
