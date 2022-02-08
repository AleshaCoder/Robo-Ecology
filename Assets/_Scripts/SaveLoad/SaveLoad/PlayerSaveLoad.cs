using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveLoad : MonoBehaviour
{
    [SerializeField] private HeroStates _states;
    [SerializeField] private Upgrade _magnet, _speed;
    private PlayerSerializator _serializer = new PlayerSerializator();

    private void Save(int money)
    {
            string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
            string _filePath = $"/{Serialization.PLAYER_PATH}{Serialization.PLAYER_EXTENTION}";
            PlayerData data = new PlayerData();
            data.Money = Money.Count;
            data.Speed = _states.Speed;
            data.RubbishCapacity = _states.MaxRubbishCapacity;
            data.MagnetStrength = _states.MagnetStrength;
            data.SpeedUpgrade = _speed.CurrentLevel;
            data.MagnetUpgrade = _magnet.CurrentLevel;
            _serializer.Save(data, _path, _filePath);        
    }

    public void Load()
    {
        try
        {
            string _path = $"{Application.persistentDataPath}/{Serialization.SPAWNERS_PATH}";
            string _filePath = $"/{Serialization.PLAYER_PATH}{Serialization.PLAYER_EXTENTION}";
            var data = _serializer.LoadPlayerData(_path, _filePath, PlayerSerializator.SerializerType.Player);
            if (data == null)
                return;
            _states.AddSpeed(data.Speed - _states.Speed);
            _states.AddMagnetStrength(data.MagnetStrength - _states.MagnetStrength);
            _states.AddMaxRubbishCapacity(data.RubbishCapacity - _states.MaxRubbishCapacity);
            _speed.SetCurrentLevel(data.SpeedUpgrade);
            _magnet.SetCurrentLevel(data.MagnetUpgrade);
            Money.AddMoney(data.Money - Money.Count);
        }
        catch { }
    }
    private void Start()
    {
        Money.AddMoney(0);
    }

    private void OnApplicationPause(bool pause)
    {
        Save(0);
    }

    private void OnApplicationQuit()
    {
        Save(0);
    }
}
