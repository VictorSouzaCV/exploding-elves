using UnityEngine;
using AYellowpaper.SerializedCollections;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{

    [CreateAssetMenu(fileName = "ElvesConfig", menuName = "ExplodingElves/ElvesConfig")]
    public class ElvesConfig : ScriptableObject
    {
        [SerializedDictionary]
        public SerializedDictionary<ElfType, ElfConfig> ElfConfigByType;
    }
}