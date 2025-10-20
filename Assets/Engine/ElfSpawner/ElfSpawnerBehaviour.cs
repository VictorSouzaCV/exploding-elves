using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    public class ElfSpawnerBehaviour : MonoBehaviour, IElfSpawnerAdapter
    {
        public Action<float> OnTick { get; set; }
        public Action<float> OnSpawnFrequencyChanged { get; set; }
        [SerializeField] private Slider _spawnTimerSlider;
        [SerializeField] private Transform _spawnRoot;
        [SerializeField] private int _poolDefaultCapacity = 10;
        [SerializeField] private int _poolMaxCapacity = 100;

        private ObjectPool<ElfBehaviour> _elfPool;
        private ElfBehaviour _elfPrefab;

        private void Start()
        {
            _spawnTimerSlider.onValueChanged.AddListener(OnSpawnFrequencyInputChanged);
        }

        public void InitializePool(ElfBehaviour elfPrefab)
        {
            _elfPrefab = elfPrefab;
            _elfPool = new ObjectPool<ElfBehaviour>(
                createFunc: () => CreatePooledElf(),
                actionOnGet: (elf) => OnTakeFromPool(elf),
                actionOnRelease: (elf) => OnReturnToPool(elf),
                actionOnDestroy: (elf) => DestroyPooledElf(elf),
                collectionCheck: false,
                defaultCapacity: _poolDefaultCapacity,
                maxSize: _poolMaxCapacity
            );
        }

        private ElfBehaviour CreatePooledElf()
        {
            var elf = Instantiate(_elfPrefab, _spawnRoot);
            elf.gameObject.SetActive(false);
            return elf;
        }

        private void OnTakeFromPool(ElfBehaviour elf)
        {
            elf.gameObject.SetActive(true);
            elf.transform.position = transform.position;
            elf.transform.rotation = Quaternion.identity;
            elf.OnExplode += OnElfExplode;
        }

        private void OnReturnToPool(ElfBehaviour elf)
        {
            elf.gameObject.SetActive(false);
            elf.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            elf.OnExplode -= OnElfExplode;
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
            if (_elfPool == null)
            {
                InitializePool(elf as ElfBehaviour);
            }

            return _elfPool.Get();
        }

        private void OnElfExplode(IElfAdapter elf)
        {
            _elfPool.Release(elf as ElfBehaviour);
        }

        private void DestroyPooledElf(ElfBehaviour elf)
        {
            if (elf != null)
                Destroy(elf.gameObject);
        }

        private void OnDestroy()
        {
            _elfPool?.Clear();
        }
    }
}