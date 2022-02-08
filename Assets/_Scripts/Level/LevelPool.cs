using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPool : MonoBehaviour
{
    private void ValidateIDs(Level[] levels)
    {
        for (int i = 0; i < levels.Length - 1; i++)
        {
            for (int j = i + 1; j < levels.Length; j++)
            {
                if (levels[i] == levels[j])
                    continue;
                if (levels[i].ID == levels[j].ID)
                    levels[j].ChangeID(levels[j].ID + 1);
            }
        }
    }
    public Level[] GetLevels()
    {
        Level[] levels = FindObjectsOfType(typeof(Level)) as Level[];//а чё бы и нет, ваще пох
        ValidateIDs(levels);
        return levels;
    }
}
