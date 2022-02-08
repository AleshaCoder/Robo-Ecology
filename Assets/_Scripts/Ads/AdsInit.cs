using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInit : MonoBehaviour
{
    private string _gameID = "4422009";
    private bool _testMode = true;

    private void Start()
    {
        Advertisement.Initialize(_gameID, _testMode);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (Advertisement.IsReady("Main_Banner") != true)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show("Main_Banner");
    }
}
