using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaveLoad : MonoBehaviour
{
    [SerializeField] private LevelPool _pool;
    private PlayerSerializator _serializer = new PlayerSerializator();
    private int _size = 0;
    private void Save()
    {
        string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
        string _filePath = $"/{Serialization.LEVELS_PATH}{Serialization.LEVELS_EXTENTION}";
        var pool = _pool.GetLevels();
        var data = new LevelData();
        data.Size = pool.Length;
        for (int i = 0; i < data.Size; i++)
        {
            data.IsUnlocked.Add(pool[i].IsUnlock);
            data.ID.Add(pool[i].ID);
        }
        _serializer.Save(data, _path, _filePath);
    }

    public void Load()
    {
        string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
        string _filePath = $"/{Serialization.LEVELS_PATH}{Serialization.LEVELS_EXTENTION}";
        var data = _serializer.LoadLevelData(_path, _filePath, PlayerSerializator.SerializerType.Levels);
        if (data == null)
        {
            return;
        }
        var pool = _pool.GetLevels();
        _size = data.Size;
        int j = 0;
        bool hasID = true;
        for (int i = 0; i < _size; i++)
        {
            j = i;
            hasID = true;
            while (pool[j].ID != data.ID[i])
            {
                j++;
                if (j >= _size) j = 0;
                if (j == i)
                {
                    hasID = false;
                    break;
                }
            }
            if (hasID == false) continue;

            //if (data.IsUnlocked[i])
            //    pool[j].Unlock(false);
        }
    }

    //public void Start()
    //{
    //    var pool = _pool.GetLevels();
    //    for (int i = 0; i < _size; i++)
    //    {
    //        if (pool[i].IsUnlock)
    //            pool[i].Unlock(false);
    //    }
    //}

    private void OnApplicationPause(bool pause)
    {
        Save();
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}
