using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishSpawnersIndicator : MonoBehaviour
{
    public delegate void SpawnerIndicatingHandler(RubbishRingSpawner spawner);
    public event SpawnerIndicatingHandler SpawnerHasFound;

    private List<RubbishRingSpawner> _rubbishRingSpawners = new List<RubbishRingSpawner>();

    public RubbishRingSpawner Spawner { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(typeof(RubbishRingSpawner), out Component component))
        {
            RubbishRingSpawner r = component as RubbishRingSpawner;
            if (r.Killing == false)
            {
                SpawnerHasFound?.Invoke(r);
            }
        }
    }
}
