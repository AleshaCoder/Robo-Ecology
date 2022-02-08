using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BoostAds : AchievementGiver, IUnityAdsListener
{
    private string _gameID = "4422009";
    private bool _testMode = true;
    private static string _placementID = "Rewarded_Android";
    private static bool _canShow = false;

    public void Show()
    {
        if (_canShow == true)
        {
            Advertisement.Show(_placementID);
            Achievement.AddFullness(1);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        _canShow = false;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        _canShow = false;
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (_placementID == placementId)
            _canShow = true;
    }

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_gameID, _testMode);
        Achievement = AchievmentCollection.instance.GetAchievement(AchievmentCollection.Boost);
    }
}
