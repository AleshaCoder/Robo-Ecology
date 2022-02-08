using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RubbishCollector), typeof(RubbishToMoneyConverter))]
public class RubbishRecycling : AchievementGiver
{
    [SerializeField] private RubbishHelper _template;
    [SerializeField] private Path _walkPath;
    [SerializeField] private Path _homePath;
    [SerializeField] private int _helpersCount;
    [SerializeField] private float _spawnTime;
    [SerializeField] private Point _spawnPoint;
    private List<RubbishHelper> _rubbishHelpers = new List<RubbishHelper>();
    private RubbishCollector _rubbishCollector;
    private RubbishToMoneyConverter _converter;
    private IEnumerator _spawner;

    private IEnumerator SpawnHelpers()
    {
        var time = new WaitForSeconds(_spawnTime);
        while (_helpersCount>0)
        {
            yield return time;
            _helpersCount--;
            RubbishHelper helper = Instantiate(_template, _spawnPoint.transform);
            helper.SetPaths(_walkPath, _homePath);
            helper.transform.localPosition = new Vector3(0, 1f, 0);
            helper.gameObject.transform.parent = null;
            _rubbishHelpers.Add(helper);
            Achievement.AddFullness(1);
        }
    }

    private void OnEnable()
    {
        Achievement = AchievmentCollection.instance.GetAchievement(AchievmentCollection.Helpers);
        _spawner = SpawnHelpers();
        StartCoroutine(_spawner);
    }
}
