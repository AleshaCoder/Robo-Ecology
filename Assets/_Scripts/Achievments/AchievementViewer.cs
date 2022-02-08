using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementViewer : MonoBehaviour
{
    [SerializeField] private AchievementView _prefabUI;

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        var achievements = AchievmentCollection.instance.GetAchievements();
        foreach (var achievement in achievements)
        {
            var view = Instantiate(_prefabUI, transform);
            view.Fill(achievement);
        }
    }
}
