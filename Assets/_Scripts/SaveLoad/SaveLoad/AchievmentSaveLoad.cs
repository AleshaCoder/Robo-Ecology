using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievmentSaveLoad : MonoBehaviour
{
    private PlayerSerializator _serializer = new PlayerSerializator();

    private void Save()
    {
        string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
        string _filePath = $"/{Serialization.ACHIEVEMENT_PATH}{Serialization.ACHIEVEMENT_EXTENTION}";
        var pool = AchievmentCollection.instance.GetAchievements().ToList();
        var data = new AchievementData();
        data.Size = pool.Count;
        for (int i = 0; i < data.Size; i++)
        {
            data.IDs.Add(pool[i].ID);
            data.CurrentLevels.Add(pool[i].Level);
            data.CurrrentRewards.Add(pool[i].Reward);
            data.CurrrentFullness.Add(pool[i].CurrentFullness);            
        }
        _serializer.Save(data, _path, _filePath);
    }

    private IEnumerator WaitAchievmentCollection()
    {
        while (AchievmentCollection.instance == null)
            yield return null;

    }

    public void Load()
    {
        try
        {
            string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
            string _filePath = $"/{Serialization.ACHIEVEMENT_PATH}{Serialization.ACHIEVEMENT_EXTENTION}";

            var data = _serializer.LoadAchievementData(_path, _filePath, PlayerSerializator.SerializerType.Achievements);
            if (data == null)
            {
                return;
            }
            var pool = AchievmentCollection.instance.GetAchievements().ToList();
            int size = data.Size;
            int j = 0;
            bool hasID = true;
            for (int i = 0; i < size; i++)
            {
                j = i;
                hasID = true;                
                while (pool[j].ID != data.IDs[i])
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

                pool[j].Load(data.CurrentLevels[i], data.CurrrentFullness[i], data.CurrrentRewards[i]);
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
