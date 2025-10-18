using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using ExplodingElves.Core;
using ExplodingElves.Engine;

public class SceneOrchestrator : MonoBehaviour
{
    [SerializeField] private ElvesConfig _elvesConfig;

    [SerializeField, SerializedDictionary] private SerializedDictionary<ElfType, ElfSpawnerBehaviour> _elfSpawnerByType;
    List<IDisposable> _disposables = new List<IDisposable>();
    ElfHitController _elfHitController = new ElfHitController();

    private void Start()
    {
        foreach (var elfSpawner in _elfSpawnerByType)
        {
            var elfSpawnerDomain = new ElfSpawnerDomain(elfSpawner.Value, _elvesConfig.ElfConfigByType[elfSpawner.Key]);
            // TODO there may be a memory leak. I need to find a way to unsubscribe from the elfSpawnerDomain.OnElfSpawned event.
            elfSpawnerDomain.OnElfSpawned += _elfHitController.AddElf;
            _disposables.Add(elfSpawnerDomain);
        }
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}
