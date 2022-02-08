using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishSaveLoad : MonoBehaviour
{
    [SerializeField] private RubbishPool _pool;
    private PlayerSerializator _serializer = new PlayerSerializator();
    [SerializeField] private Rubbish[] _rubbishPrefabs;

    private void Save()
    {
        {
            string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
            string _filePath = $"/{Serialization.RUBBISH_PATH}{Serialization.RUBBISH_EXTENTION}";
            var pool = _pool.GetRubbishes();
            var data = new RubbishData();
            data.Size = pool.Length;
            for (int i = 0; i < data.Size; i++)
            {
                data.Type.Add((byte)pool[i].RubbishType);
                data.Position.Add(pool[i].transform.position);
                data.Hiden.Add(pool[i].Hiden);
            }
            _serializer.Save(data, _path, _filePath);
        }
    }

    public void Load()
    {
        try
        {
            string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
            string _filePath = $"/{Serialization.RUBBISH_PATH}{Serialization.RUBBISH_EXTENTION}";
            var data = _serializer.LoadRubbishData(_path, _filePath, PlayerSerializator.SerializerType.Buildings);
            if (data == null)
            {
                return;
            }
            int size = data.Size;
            for (int i = 0; i < size; i++)
            {
                if (data.Hiden[i])
                    continue;
                for (int j = 0; j < _rubbishPrefabs.Length; j++)
                {
                    if (_rubbishPrefabs[j].RubbishType == (RubbishType)data.Type[i])
                    {
                        var rubbish = Instantiate(_rubbishPrefabs[j], data.Position[i], Quaternion.Euler(
                            new Vector3(Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f))));
                        _pool.AddRubbish(rubbish);
                    }
                }
            }
        }
        catch { }
    }

    private void OnApplicationPause(bool pause)
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
