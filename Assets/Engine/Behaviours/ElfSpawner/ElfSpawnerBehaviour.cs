using System;
using UnityEngine;
using UnityEngine.UI;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    public class ElfSpawnerBehaviour : MonoBehaviour, IElfSpawnerAdapter
    {
        public Action<float> OnTick { get; set; }
        public Action<float> OnSpawnFrequencyChanged { get; set; }
        [SerializeField] private Slider _spawnTimerSlider;
        [SerializeField] private Transform _spawnRoot;

        private void Start()
        {
            _spawnTimerSlider.onValueChanged.AddListener(OnSpawnFrequencyInputChanged);
        }

        void FixedUpdate()
        {
            OnTick?.Invoke(Time.time);
        }

        private void OnSpawnFrequencyInputChanged(float value)
        {
            OnSpawnFrequencyChanged?.Invoke(value);
        }

        public IElfAdapter Spawn(IElfAdapter elf)
        {
            return Instantiate(elf as ElfBehaviour, transform.position, Quaternion.identity, _spawnRoot);
        }
    }
}