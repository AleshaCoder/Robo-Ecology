using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionSaveData : MonoBehaviour
{
    [SerializeField] private int _version;
    [SerializeField] private PlayerSaveLoad _playerSaveLoad;
    [SerializeField] private SpawnersSaveLoad _spawnersSaveLoad;
    [SerializeField] private RubbishSaveLoad _rubbishSaveLoad;
    [SerializeField] private LevelSaveLoad _levelSaveLoad;
    [SerializeField] private BuildingsSaveData _buildingsSaveLoad;
    [SerializeField] private AchievmentSaveLoad _achievementSaveLoad;
    private PlayerSerializator _serializer = new PlayerSerializator();
    private bool WasLoad = false;


    private void Save()
    {
        string _path = $"{Application.persistentDataPath}/{Serialization.VERSION_PATH}";
        string _filePath = $"/{Serialization.VERSION_PATH}{Serialization.VERSION_EXTENTION}";
        VersionData data = new VersionData();
        data.Version = _version;
        _serializer.Save(data, _path, _filePath);
    }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        try
        {
            string _path = $"{Application.persistentDataPath}/{Serialization.VERSION_PATH}";
            string _filePath = $"/{Serialization.VERSION_PATH}{Serialization.VERSION_EXTENTION}";
            var data = _serializer.LoadVersionData(_path, _filePath, PlayerSerializator.SerializerType.Player);
            if (data == null)
                return;
            if (data.Version == _version)
            {
                _playerSaveLoad.Load();
                _buildingsSaveLoad.Load();
                _rubbishSaveLoad.Load();
                _levelSaveLoad.Load();
                _achievementSaveLoad.Load();
            }
        }
        catch { }        
    }

    private void Start()
    {
        string _path = $"{Application.persistentDataPath}/{Serialization.VERSION_PATH}";
        string _filePath = $"/{Serialization.VERSION_PATH}{Serialization.VERSION_EXTENTION}";
        var data = _serializer.LoadVersionData(_path, _filePath, PlayerSerializator.SerializerType.Player);
        if (data == null)
            return;
        if (data.Version == _version)
        {
            _spawnersSaveLoad.Load();
        }
        WasLoad = true;
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
