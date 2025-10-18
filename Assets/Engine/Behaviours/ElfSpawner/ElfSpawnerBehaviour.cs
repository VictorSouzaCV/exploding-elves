using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    public class ElfSpawnerBehaviour : MonoBehaviour
    {
        [SerializeField] private ElfType _elfType;
        [SerializeField] private Slider _spawnTimerSlider;
        [SerializeField] private GameObject _elfPrefab;
        private float _spawnTimer;
        private float _lastSpawnTime;

        private void Start()
        {
            _spawnTimer = _spawnTimerSlider.value;
            _lastSpawnTime = Time.time;
            _spawnTimerSlider.onValueChanged.AddListener(OnSpawnTimerChanged);
        }

        private void OnSpawnTimerChanged(float value)
        {
            _spawnTimer = value;
        }

        private void FixedUpdate()
        {
            if (Time.time - _lastSpawnTime >= 1f/_spawnTimer)
            {
                _lastSpawnTime = Time.time;
                var randonPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                Instantiate(_elfPrefab, transform.position + randonPosition, Quaternion.identity);
            }
        }
    }
}