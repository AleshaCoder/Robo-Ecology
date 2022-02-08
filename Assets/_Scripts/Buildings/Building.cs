using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollectingRubbish
{
    public RubbishType Type;
    public int Count;

    public void DecreaseCount()
    {
        Count--;
    }
}
[RequireComponent(typeof(RubbishCollector), typeof(MoneyCollector))]
public class Building : AchievementGiver
{
    [System.Serializable]
    public struct BuildingRequirements
    {
        public int MoneyCount;
        [HideInInspector] public bool Achived1;
        public List<CollectingRubbish> needRubbishes;
        [HideInInspector] public bool Achived2;
        [HideInInspector] public bool Complete => Achived1 && Achived2;
    }
    [SerializeField] private int _id = 0;
    [SerializeField] private BuildingRequirements _buildingRequirements;
    [SerializeField] private GameObject _readyBuilding;
    [SerializeField] private bool _isTree;


    private RubbishCollector _collector;
    private MoneyCollector _moneyCollector;
    public bool IsReady { get; private set; }
    public int ID => _id;
    public BuildingRequirements Requirements => _buildingRequirements;

    public UnityAction<int> OnIDChanged;

    public void ChangeID(int newID)
    {
        _id = newID;
        OnIDChanged?.Invoke(_id);
    }

    public void LoadBuildingRequiremets(BuildingRequirements requirements)
    {
        _buildingRequirements = requirements;
    }

    private void Achiev()
    {
        if (_buildingRequirements.Complete)
            Build();
    }

    private void Build()
    {
        if (IsReady == true)
            return;
        IsReady = true;
        _readyBuilding.SetActive(true);
        _collector.enabled = false;
        _moneyCollector.enabled = false;
        gameObject.SetActive(false);
        Achievement?.AddFullness(1);
    }

    private void Start()
    {
        _collector = GetComponent(typeof(RubbishCollector)) as RubbishCollector;
        _collector.ChangeCollectingRubbish(_buildingRequirements.needRubbishes);
        _collector.CollectorFull += () => { _buildingRequirements.Achived1 = true; Achiev(); };

        _moneyCollector = GetComponent(typeof(MoneyCollector)) as MoneyCollector;
        _moneyCollector.ChangeNeedMoneyCount(_buildingRequirements.MoneyCount);
        _moneyCollector.CollectorFull += () => { _buildingRequirements.Achived2 = true; Achiev(); };
        Achiev();
        if (_isTree == false)
            Achievement = AchievmentCollection.instance.GetAchievement(AchievmentCollection.Buildings);
        else
            Achievement = AchievmentCollection.instance.GetAchievement(AchievmentCollection.Trees);
    }

}
