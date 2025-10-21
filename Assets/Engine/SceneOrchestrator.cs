using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using ExplodingElves.Core;
using ExplodingElves.Engine;

public class SceneOrchestrator : MonoBehaviour, IClockAdapter
{
    public float CurrentTime => Time.time;
    public Action<float> OnTick { get; set; }
    [SerializeField] private ElvesConfig _elvesConfig;

    [SerializeField, SerializedDictionary] private SerializedDictionary<ElfType, SpawnerConfig> _elfSpawnerByType;
    List<IDisposable> _disposables = new List<IDisposable>();
    ElfHitController _elfHitController = new ElfHitController();

    private void Start()
    {
        foreach (var elfSpawner in _elfSpawnerByType)
        {
            var elfSpawnerDomain = new ElfSpawnerDomain(elfSpawner.Value.Behaviour, _elvesConfig.ElfConfigByType[elfSpawner.Key], _elvesConfig.ElfAdapter, this, elfSpawner.Value.StartAngularVariation);
            _elfHitController.AddElfSpawner(elfSpawner.Key, elfSpawnerDomain);
            _disposables.Add(elfSpawnerDomain);
        }
    }

    void FixedUpdate()
    {
        OnTick?.Invoke(Time.time);
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }

    [Serializable]
    struct SpawnerConfig
    {
        public ElfSpawnerBehaviour Behaviour;
        public float StartAngularVariation;
    }
}
