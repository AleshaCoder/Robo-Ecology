using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum MagnetOwner
{
    Player,
    Helper
}
public class RubbishMagnet : MonoBehaviour
{
    [SerializeField] private float _magnetForce = 100;
    [SerializeField] private int _maxRubbishCount = 5;
    [SerializeField] private bool _autoCollect = true;
    [SerializeField] private RubbishIndicator _rubbishIndicator;
    [SerializeField] private MagnetOwner _owner = MagnetOwner.Player;
    [SerializeField] private bool _useHeroStates = true;

    private List<Rubbish> _nearestRubbish = new List<Rubbish>();

    private int _currentRubishCount => _nearestRubbish.Count;

    public MagnetOwner Owner => _owner;

    public int MaxRubbishCount => _maxRubbishCount;

    public event UnityAction<int> RubbishCountChanged;

    public List<Rubbish> GetMagnetedRubbish()
    {
        return _nearestRubbish;
    }

    private void AddRubbishForMagnetting(Rubbish rubbish)
    {
        if (_nearestRubbish.Contains(rubbish))
            return;
        if (_currentRubishCount < _maxRubbishCount)
        {
            _nearestRubbish.Add(rubbish);
            rubbish.Magnet();
            rubbish.OnHide += RemoveRubbishFromMagnetting;
            rubbish.Destroing += RemoveRubbishFromMagnetting;
            RubbishCountChanged?.Invoke(_currentRubishCount);
        }
    }

    private void RemoveRubbishFromMagnetting(Rubbish rubbish)
    {
        if (_nearestRubbish.Remove(rubbish))
        {            
            RubbishCountChanged?.Invoke(_currentRubishCount);
        }
    }

    private void ChangeMagnetStrength(int strength)
    {
        _magnetForce = strength;
    }

    private void ChangeMaxMagnetCapacity(int capacity)
    {
        _maxRubbishCount = capacity;
    }

    private void OnEnable()
    {
        if (_useHeroStates)
        {
            HeroStates.MagnetStrengthChanged += ChangeMagnetStrength;
            HeroStates.MaxRubbishCapacityChanged += ChangeMaxMagnetCapacity;
        }
        if (_autoCollect == true)
        {
            _rubbishIndicator.RubbishHasFound += AddRubbishForMagnetting;
            _rubbishIndicator.RubbishHasLost += RemoveRubbishFromMagnetting;
        }
    }

    private void OnDisable()
    {
        if (_useHeroStates)
        {
            HeroStates.MagnetStrengthChanged -= ChangeMagnetStrength;
            HeroStates.MaxRubbishCapacityChanged -= ChangeMaxMagnetCapacity;
        }
        if (_autoCollect == true)
        {
            _rubbishIndicator.RubbishHasFound -= AddRubbishForMagnetting;
            _rubbishIndicator.RubbishHasLost -= RemoveRubbishFromMagnetting;
        }
    }

    private void FixedUpdate()
    {
        if (_nearestRubbish.Count > 0)
        {
            foreach (Rubbish item in _nearestRubbish)
            {
                item.Body.velocity = (transform.position - (item.transform.position + item.Body.centerOfMass)) * _magnetForce * Time.fixedDeltaTime;
            }
        }
    }
}
