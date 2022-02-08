using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievmentCollection : MonoBehaviour
{
    public const string Rubbish = "Rubbish";
    public const string Buildings = "Buildings";
    public const string TrashRobots = "TrashRobots";
    public const string Fashion = "Fashion";
    public const string Helpers = "Helpers";
    public const string Location = "Location";
    public const string Social = "Social";
    public const string Support = "Support";
    public const string Trees = "Trees";
    public const string Boost = "Boost";

    public static AchievmentCollection instance = null;
    [SerializeField] private List<Achievement> _achievements = new List<Achievement>();

    public Achievement GetAchievement(string name)
    {
        foreach (var item in _achievements)
        {
            if (item.Name == name)
                return item;
        }
        return null;
    }

    public IReadOnlyCollection<Achievement> GetAchievements()
    {
        return _achievements;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
}
