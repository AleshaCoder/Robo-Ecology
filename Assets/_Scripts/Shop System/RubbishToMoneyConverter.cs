using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RubbishAndPrice
{
    public RubbishType RubbishType;
    public int Price;
}
[RequireComponent(typeof(RubbishCollector))]
public class RubbishToMoneyConverter : AchievementGiver
{
    [SerializeField] private List<RubbishAndPrice> _rubbishAndPrices;

    private RubbishCollector _rubbishCollector;

    private void TryConvertToMoney(Rubbish rubbish)
    {
        RubbishType rubbishType = rubbish.RubbishType;
        foreach (RubbishAndPrice item in _rubbishAndPrices)
        {
            if (item.RubbishType == rubbishType)
            {
                Money.AddMoney(item.Price);
                Achievement = Achievement ?? AchievmentCollection.instance?.GetAchievement(AchievmentCollection.Rubbish);
                Achievement?.AddFullness(1);
                break;
            }
        }
    }

    private void Awake()
    {
        _rubbishCollector = GetComponent(typeof(RubbishCollector)) as RubbishCollector;        
    }

    private void OnEnable()
    {
        _rubbishCollector.RubbishFound += TryConvertToMoney;
        Achievement = AchievmentCollection.instance?.GetAchievement(AchievmentCollection.Rubbish);
    }

    private void OnDisable()
    {
        _rubbishCollector.RubbishFound -= TryConvertToMoney;
    }
}
