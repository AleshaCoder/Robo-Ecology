using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroStates : MonoBehaviour
{
    [SerializeField] private int _speed = 5;
    [SerializeField] private int _magnetStrength = 100;
    [SerializeField] private int _maxRubbishCapacity = 5;

    public int Speed => _speed;
    public int MagnetStrength => _magnetStrength;
    public int MaxRubbishCapacity => _maxRubbishCapacity;

    public static event UnityAction<int> SpeedChanged;
    public static event UnityAction<int> MagnetStrengthChanged;
    public static event UnityAction<int> MaxRubbishCapacityChanged;

    public void AddSpeed(int count)
    {
        _speed += count;
        SpeedChanged?.Invoke(_speed);
    }

    public void AddMagnetStrength(int count)
    {
        _magnetStrength += count;
        MagnetStrengthChanged?.Invoke(_magnetStrength);
    }

    public void AddMaxRubbishCapacity(int count)
    {
        _maxRubbishCapacity += count;
        MaxRubbishCapacityChanged?.Invoke(_maxRubbishCapacity);
    }

    private void Start()
    {
        SpeedChanged?.Invoke(_speed);
        MagnetStrengthChanged?.Invoke(_magnetStrength);
        MaxRubbishCapacityChanged?.Invoke(_maxRubbishCapacity);       
    }
}
