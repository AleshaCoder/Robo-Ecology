using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Upgrade : MonoBehaviour
{
    [SerializeField] protected HeroStates _heroStates;
    [SerializeField] protected TMP_Text _price;
    [SerializeField] protected List<TMP_Text> _stats;
    [SerializeField] protected int _maxLevel = 100;
    [SerializeField] protected List<int> _upgradePrices;

    protected int _currentLevel = 1;

    public int CurrentLevel => _currentLevel;

    public abstract void MakeUpgrade();

    public void SetCurrentLevel(int level)
    {
        if (level > _maxLevel)
            return;
        _currentLevel = level;
        UpdateLevel();
    }

    protected void UpdateLevel()
    {
        _price.text = _upgradePrices[_currentLevel - 1].ToString();
    }

    private void Start()
    {
        UpdateLevel();
    }
}
