using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
public class PlayerSerializator
{
    public enum SerializerType
    {
        Player,
        Spawners,
        Buildings,
        Rubbish,
        Levels,
        Achievements
    }

    private string SelectSavePath(SerializerType serializerType)
    {
        /* switch (serializerType)
         {
             case SerializerType.Player:
                 return $"{Application.persistentDataPath}/{Serialization.PLAYER_PATH}/{Serialization.PLAYER_PATH}{Serialization.PLAYER_EXTENTION}";
             case SerializerType.Spawners:
                 return $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}/{Serialization.SPAWNERS_PATH}{Serialization.SPAWNERS_EXTENTION}";
             case SerializerType.Buildings:
                 return $"{Application.persistentDataPath}/{Serialization.BUILDINGS_PATH}/{Serialization.BUILDINGS_PATH}{Serialization.BUILDINGS_EXTENTION}";
             case SerializerType.Rubbish:
                 return $"{Application.persistentDataPath}/{Serialization.RUBBISH_PATH}/{Serialization.RUBBISH_PATH}{Serialization.RUBBISH_EXTENTION}";
             case SerializerType.Levels:
                 return $"{Application.persistentDataPath}/{Serialization.LEVELS_PATH}/{Serialization.LEVELS_PATH}{Serialization.LEVELS_EXTENTION}";
             default:*/
        return null;
        /*}*/
    }

    public void DeleteCurrentData(string path)
    {
        var savePath = path;
        if (File.Exists(path))
            File.Delete(path);
    }

    public void Save(IData data, string path, string filepath)
    {
        DeleteCurrentData(path + filepath);
        Directory.CreateDirectory(path);
        File.WriteAllBytes(path + filepath, data.Serialize());
    }

    public T Load<T>(SerializerType serializerType)
    {
        var savePath = SelectSavePath(serializerType);
        if (File.Exists(savePath) == false)
            return (T)Convert.ChangeType(null, typeof(T));

        switch (serializerType)
        {
            case SerializerType.Player:
                var data = PlayerData.Deserialize(File.ReadAllBytes(savePath));
                return (T)Convert.ChangeType(data, typeof(T));
            case SerializerType.Spawners:
                var data1 = SpawnersData.Deserialize(File.ReadAllBytes(savePath));
                return (T)Convert.ChangeType(data1, typeof(T));
            /*            case SerializerType.Buildings:
                            return _buildingsSavePath;
                        case SerializerType.Rubbish:
                            return _rubbishSavePath;
                        case SerializerType.Levels:
                            return _levelsSavePath;*/
            default:
                return (T)Convert.ChangeType(null, typeof(T));
        }
    }

    public PlayerData LoadPlayerData(string path, string filepath, SerializerType serializerType)
    {
        if (File.Exists(path + filepath) == false)
            return null;
        return PlayerData.Deserialize(File.ReadAllBytes(path + filepath));
    }
    public SpawnersData LoadSpawnersData(string path, string filepath, SerializerType serializerType)
    {
        if (File.Exists(path + filepath) == false)
            return null;
        return SpawnersData.Deserialize(File.ReadAllBytes(path + filepath));
    }
    public BuildingsData LoadBuildingsData(string path, string filepath, SerializerType serializerType)
    {
        if (File.Exists(path + filepath) == false)
            return null;
        return BuildingsData.Deserialize(File.ReadAllBytes(path + filepath));
    }
    public RubbishData LoadRubbishData(string path, string filepath, SerializerType serializerType)
    {
        if (File.Exists(path + filepath) == false)
            return null;
        return RubbishData.Deserialize(File.ReadAllBytes(path + filepath));
    }
    public LevelData LoadLevelData(string path, string filepath, SerializerType serializerType)
    {
        if (File.Exists(path + filepath) == false)
            return null;
        return LevelData.Deserialize(File.ReadAllBytes(path + filepath));
    }
    public VersionData LoadVersionData(string path, string filepath, SerializerType serializerType)
    {
        if (File.Exists(path + filepath) == false)
            return null;
        return VersionData.Deserialize(File.ReadAllBytes(path + filepath));
    }
    public AchievementData LoadAchievementData(string path, string filepath, SerializerType serializerType)
    {
        if (File.Exists(path + filepath) == false)
            return null;
        return AchievementData.Deserialize(File.ReadAllBytes(path + filepath));
    }
}
