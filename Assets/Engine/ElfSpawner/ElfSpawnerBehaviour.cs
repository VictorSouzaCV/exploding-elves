using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using ExplodingElves.Core;

namespace ExplodingElves.Engine
{
    public class ElfSpawnerBehaviour : MonoBehaviour, IElfSpawnerAdapter
    {
        public Action<float> OnSpawnFrequencyChanged { get; set; }
        [SerializeField] private Slider _spawnTimerSlider;
        [SerializeField] private Transform _spawnRoot;
        [SerializeField] private int _poolDefaultCapacity = 50;
        [SerializeField] private int _poolMaxCapacity = 1000;
        [SerializeField] private Image _spawnerImage;

        private ObjectPool<ElfBehaviour> _elfPool;
        private ElfBehaviour _elfPrefab;

        private void Start()
        {
            _spawnTimerSlider.onValueChanged.AddListener(OnSpawnFrequencyInputChanged);
        }

        public void SetColor((float r, float g, float b, float a) color)
        {
            _spawnerImage.color = new Color(color.r, color.g, color.b, color.a);
        }

        public IElfAdapter Spawn(IElfAdapter elf)
        {
            if (_elfPool == null)
            {
                InitializePool(elf as ElfBehaviour);
            }

            var elfBehaviour = _elfPool.Get();
            return elfBehaviour;
        }

        private void OnSpawnFrequencyInputChanged(float value)
        {
            OnSpawnFrequencyChanged?.Invoke(value);
        }

        private void OnElfExplode(IElfAdapter elf)
        {
            _elfPool.Release(elf as ElfBehaviour);
        }

        private void OnDestroy()
        {
            _elfPool?.Clear();
        }

        #region Object Pool
        private void InitializePool(ElfBehaviour elfPrefab)
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
            var elf = Instantiate(_elfPrefab, transform.position, Quaternion.identity, _spawnRoot);
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
            elf.OnExplode -= OnElfExplode;
        }

        private void DestroyPooledElf(ElfBehaviour elf)
        {
            elf.OnExplode -= OnElfExplode;
            if (elf != null)
                Destroy(elf.gameObject);
        }
        #endregion
    }
}