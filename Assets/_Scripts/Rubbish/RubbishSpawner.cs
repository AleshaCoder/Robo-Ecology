using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RubbishSpawner
{
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private float _rubbishVelocity;

    private float _timeSinceLastSpawn = 0;
    private bool _canSpawn = true;

    public GameObject Spawner { get; private set; }
    public float RubbishVelocity => _rubbishVelocity;
    public float TimeBetweenSpawns => _timeBetweenSpawns;
    public UnityAction<GameObject, float> OnSpawn;

    public RubbishSpawner(GameObject spawner, float rubbishVelocity, float timeBetweenSpawns) 
    { 
        Spawner = spawner;
        _timeBetweenSpawns = timeBetweenSpawns;
        _rubbishVelocity = rubbishVelocity;
    }

    public void Stop()
    {
        _canSpawn = false;
        _timeSinceLastSpawn = 0;
    }

    public void Play()
    {
        _canSpawn = true;
    }

    private void SpawnRubish()
    {
        OnSpawn?.Invoke(Spawner, RubbishVelocity);
    }

    public void TrySpawn(float delta)
    {
        if (_canSpawn == true)
        {
            _timeSinceLastSpawn += delta;
            if (_timeSinceLastSpawn >= _timeBetweenSpawns)
            {
                _timeSinceLastSpawn -= _timeBetweenSpawns;
                SpawnRubish();
            }
        }
    }
}
