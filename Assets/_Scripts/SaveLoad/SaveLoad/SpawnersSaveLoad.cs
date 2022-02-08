using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersSaveLoad : MonoBehaviour
{
    [SerializeField] private RubbishSpawnersPool _pool;
    [SerializeField] private GameObject _pathPrefab;
    [SerializeField] private GameObject[] _spawnerPrefab;
    private PlayerSerializator _serializer = new PlayerSerializator();
    private bool WasLoad = false;
    private void Save()
    {
        //try
        //{
        string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
        string _filePath = $"/{Serialization.SPAWNERS_PATH}{Serialization.SPAWNERS_EXTENTION}";
        var pool = _pool.GetPool();
        var data = new SpawnersData();
        data.Size = pool.Count;
        foreach (var item in pool)
        {
            //if (item.gameObject.activeInHierarchy == false)
            //    continue;
            data.Positions.Add(item.transform.position);
            data.CanMove.Add(item.GetPathMovement().CanMove);
            data.Types.Add((byte)item.Type);
            if (item.GetPath() == null)
            {
                data.PointsCount.Add(0);
            }
            else
            {
                var points = item.GetPath().GetPoints();
                data.PointsCount.Add(points.Count);
                foreach (var point in points)
                {
                    data.Points.Add(point);
                }
            }
            data.Active.Add(item.Killing);
            Debug.Log(item.Killing);
        }
        _serializer.Save(data, _path, _filePath);
        //} catch
        //{
        //}
    }

    public void Load()
    {
        Debug.Log(Application.persistentDataPath);
        string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
        string _filePath = $"/{Serialization.SPAWNERS_PATH}{Serialization.SPAWNERS_EXTENTION}";
        var data = _serializer.LoadSpawnersData(_path, _filePath, PlayerSerializator.SerializerType.Spawners);
        if (data == null)
        {
            // StartCoroutine(SaveData());
            return;
        }
        var pool = _pool.GetPool();
        foreach (var item in pool)
        {
            //if (item.gameObject.activeInHierarchy == false)
            //    continue;
            Destroy(item.gameObject);
            if (item.GetPath() != null)
                Destroy(item.GetPath().gameObject);
        }
        pool.Clear();

        int size = data.Size;
        int point = 0;
        for (int i = 0; i < size; i++)
        {
            //if (data.Active[i] == false)
            //    continue;
            var spawner = Instantiate(_spawnerPrefab[data.Types[i]], data.Positions[i], Quaternion.identity);
            var pathMovement = spawner.GetComponent(typeof(PathMovement)) as PathMovement;
            var pathGO = Instantiate(_pathPrefab, Vector3.zero, Quaternion.identity);
            var path = pathGO.GetComponent(typeof(Path)) as Path;
            List<Vector3> points = new List<Vector3>();
            for (int j = point; j < point + data.PointsCount[i]; j++)
            {
                points.Add(data.Points[j]);
            }
            point += data.PointsCount[i];
            pathGO.transform.parent = transform;

            path.SetPoints(points);
            pathMovement.SetPath(path);
            pathMovement.StartMoving();
            spawner.transform.parent = transform;
            spawner.GetComponent<RubbishRingSpawner>().Init(data.Active[i]);
        }
        //StartCoroutine(SaveData());
        WasLoad = true;
    }

    private IEnumerator TryLoad()
    {
        yield return new WaitForEndOfFrame();
        if (WasLoad == false)
        {
            WasLoad = true;
        }
    }

    private void Start()
    {
        StartCoroutine(TryLoad());
    }

    private void OnApplicationPause(bool pause)
    {
        if (WasLoad == true)
            Save();
    }

    private void OnApplicationQuit()
    {
        if (WasLoad == true)
            Save();
    }
}
