using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker : AchievementGiver
{
    [SerializeField] private Level _level;

    private void OnEnable()
    {
        Achievement = AchievmentCollection.instance?.GetAchievement(AchievmentCollection.Location);
        if (_level.IsUnlock)
            return;
        _level.Unlock(true);
        Achievement?.AddFullness(1);
    }
}
