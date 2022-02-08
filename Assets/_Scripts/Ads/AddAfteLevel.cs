using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AddAfteLevel : MonoBehaviour, IUnityAdsListener
{
    private string _gameID = "4422009";
    private bool _testMode = true;
    private static string _placementID = "After_Building";
    private static bool _canShow = false;
    private static bool _timerEnd = true;

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_gameID, _testMode);
    }

    private IEnumerator WaitTime()
    {
        _timerEnd = false;
        yield return new WaitForSeconds(180);
        _timerEnd = true;
    }

    public static void Show()
    {
        if (_canShow == _timerEnd == true)
        {
            Advertisement.Show(_placementID);            
        }
        if (_canShow == false)
            Debug.Log("Рекламы нет");
        if (_timerEnd == false)
            Debug.Log("Дурак, время еще не прошло");

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
        StartCoroutine(WaitTime());
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (_placementID == placementId)
            _canShow = true;
    }
}
