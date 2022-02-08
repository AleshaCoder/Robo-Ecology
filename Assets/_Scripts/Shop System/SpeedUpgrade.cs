using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : Upgrade
{
    public override void MakeUpgrade()
    {
        if (_currentLevel >= _maxLevel)
            return;

        if (Money.TryGetMoney(_upgradePrices[_currentLevel - 1]))
        {
            _currentLevel++;
            UpdateLevel();
            _heroStates.AddSpeed(1);
        }
    }

    private void OnEnable()
    {
        HeroStates.SpeedChanged += (int count) => _stats[0].text = count.ToString();
    }

    private void OnDisable()
    {
        HeroStates.SpeedChanged -= (int count) => _stats[0].text = count.ToString();
    }
}
