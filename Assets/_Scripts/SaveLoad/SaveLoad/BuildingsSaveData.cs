using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsSaveData : MonoBehaviour
{
    [SerializeField] private BuildingsPool _pool;
    private PlayerSerializator _serializer = new PlayerSerializator();

    private void Save()
    {
        string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
        string _filePath = $"/{Serialization.BUILDINGS_PATH}{Serialization.BUILDINGS_EXTENTION}";
        var pool = _pool.GetBuildings();
        var data = new BuildingsData();
        data.Size = pool.Length;
        for (int i = 0; i < data.Size; i++)
        {
            data.Active.Add(pool[i].Requirements.Complete);
            data.RubbishListCount.Add(pool[i].Requirements.needRubbishes.Count);
            for (int j = 0; j < pool[i].Requirements.needRubbishes.Count; j++)
            {
                data.RubbishType.Add((byte)pool[i].Requirements.needRubbishes[j].Type);
                data.RubbishCount.Add(pool[i].Requirements.needRubbishes[j].Count);
            }
            data.Coin.Add(pool[i].Requirements.MoneyCount);
            data.ID.Add(pool[i].ID);
        }
        _serializer.Save(data, _path, _filePath);
    }

    public void Load()
    {
        try
        {
            string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
            string _filePath = $"/{Serialization.BUILDINGS_PATH}{Serialization.BUILDINGS_EXTENTION}";
            Debug.Log(_path);
            var data = _serializer.LoadBuildingsData(_path, _filePath, PlayerSerializator.SerializerType.Buildings);
            if (data == null)
            {
                return;
            }
            var pool = _pool.GetBuildings();
            int size = data.Size;
            int j = 0;
            bool hasID = true;
            for (int i = 0; i < size; i++)
            {
                j = i;
                hasID = true;
                Building.BuildingRequirements requirements = new Building.BuildingRequirements();
                while (pool[j].ID != data.ID[i])
                {
                    j++;
                    if (j >= size) j = 0;
                    if (j == i)
                    {
                        hasID = false;
                        break;
                    }
                }
                if (hasID == false) continue;

                if (data.Active[i] == true)
                {
                    requirements.Achived1 = true;
                    requirements.Achived2 = true;
                }

                requirements.MoneyCount = data.Coin[i];
                requirements.needRubbishes = new List<CollectingRubbish>(data.RubbishListCount[i]);
                for (int k = 0; k < data.RubbishListCount[i]; k++)
                {
                    requirements.needRubbishes[k] = new CollectingRubbish();
                    requirements.needRubbishes[k].Count = data.RubbishCount[i];
                    requirements.needRubbishes[k].Type = (RubbishType)data.RubbishType[i];
                }
                pool[j].LoadBuildingRequiremets(requirements);
            }
        }
        catch
        { }
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
