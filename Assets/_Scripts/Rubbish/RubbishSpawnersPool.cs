using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnerType : byte
{
    Type1,
    Type2,
    Type3,
    Type4
}

public class RubbishSpawnersPool : MonoBehaviour
{
    [SerializeField] private List<RubbishRingSpawner> _spawners = new List<RubbishRingSpawner>();

    public List<RubbishRingSpawner> GetPool()
    {
        _spawners.Clear();
        RubbishRingSpawner[] spawners = FindObjectsOfType(typeof(RubbishRingSpawner), true) as RubbishRingSpawner[];        
        _spawners.AddRange(spawners);
        return _spawners;
    }
}
