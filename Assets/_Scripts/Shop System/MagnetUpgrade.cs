using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetUpgrade : Upgrade
{
    public override void MakeUpgrade()
    {
        if (_currentLevel >= _maxLevel)
            return;

        if (Money.TryGetMoney(_upgradePrices[_currentLevel - 1]))
        {
            _currentLevel++;
            UpdateLevel();
            _heroStates.AddMagnetStrength(20);
            _heroStates.AddMaxRubbishCapacity(1);
        }
    }
    private void OnEnable()
    {
        HeroStates.MagnetStrengthChanged += (int count) => _stats[1].text = count.ToString();
        HeroStates.MaxRubbishCapacityChanged += (int count) => _stats[0].text = count.ToString();
    }

    private void OnDisable()
    {
        HeroStates.MagnetStrengthChanged -= (int count) => _stats[1].text = count.ToString();
        HeroStates.MaxRubbishCapacityChanged -= (int count) => _stats[0].text = count.ToString();
    }
}
