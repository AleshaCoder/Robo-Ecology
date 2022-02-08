using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonationCollector : MonoBehaviour
{
    [SerializeField] private int _moneyPerTime = 1;
    [SerializeField] private int _collectingTime = 10;
    private IEnumerator _collector;
    private IEnumerator CollectMoney()
    {
        var time = new WaitForSeconds(_collectingTime);
        while (true)
        {
            yield return time;
            Money.AddMoney(_moneyPerTime);
        }
    }

    private void OnEnable()
    {
        _collector = CollectMoney();
        StartCoroutine(_collector);
    }

    private void OnDisable()
    {
        StopCoroutine(_collector);
    }
}