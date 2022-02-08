using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AchievementGiver : MonoBehaviour
{
    protected Achievement Achievement;

    protected void UpgradeAchievement() { }
}

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement", order = 51)]
public class Achievement : ScriptableObject
{
    [Header("General")]
    [SerializeField] private string _name;
    [SerializeField] private int _id;

    [Space]
    [Header("Description")]
    [SerializeField] private string _headerName;
    [SerializeField] private string _description;

    [Space]
    [Header("Values")]
    [SerializeField] [Range(0, 10)] private int _maxLevel;
    [SerializeField] [Range(0, 10)] private int _currentLevel;
    [SerializeField] private List<LevelInfo> _levelsInfo = new List<LevelInfo>();
    [Space]
    [SerializeField] private bool _end;
    [SerializeField] private bool _flow;

    private int _accumulatedReward = 0;

    public string Name => _name;
    public int ID => _id;
    public string HeaderName => _headerName;
    public string Description => _description;
    public int Level => _currentLevel;
    public int CurrentFullness => _levelsInfo[_currentLevel].CurrentFullness;
    public int MaxFullness => _levelsInfo[_currentLevel].MaxFullness;
    public int Reward => _accumulatedReward;
    public bool End => _end;

    public void AddFullness(int fullness)
    {
        while (fullness > 0)
        {
            _levelsInfo[_currentLevel].AddFulness(fullness, out fullness);

            if (_levelsInfo[_currentLevel].Complete == false) break;            

            if (_maxLevel == _currentLevel)
            {
                _end = true;
                break;
            }

            if (_flow)
            {
                _levelsInfo[_currentLevel + 1].AddFulness(_levelsInfo[_currentLevel].MaxFullness, out int number);
            }

            _accumulatedReward += _levelsInfo[_currentLevel].Reward;
            _currentLevel++;
        }
    }

    public void Load(int level, int fullness, int reward)
    {
        _currentLevel = level;
        _levelsInfo[_currentLevel].AddFulness(fullness - _levelsInfo[_currentLevel].CurrentFullness, out int number);
        _accumulatedReward = reward;
    }

    public int TakeReward()
    {
        int reward = Reward;
        _accumulatedReward = 0;
        return reward;
    }

    private void OnValidate()
    {
        if (_currentLevel > _maxLevel)
            _currentLevel = _maxLevel;
    }

    [System.Serializable]
    private class LevelInfo
    {
        [SerializeField] private int _maxFullness;
        [SerializeField] private int _currentFullness;
        [SerializeField] private int _reward;

        public int MaxFullness => _maxFullness;
        public int CurrentFullness => _currentFullness;
        public int Reward => _reward;
        public bool Complete => _currentFullness >= _maxFullness;

        public void AddFulness(int fullness, out int remainder)
        {
            _currentFullness += fullness;
            remainder = Complete ? (_currentFullness - _maxFullness) : 0;
        }
    }
}
