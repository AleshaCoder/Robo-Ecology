using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyCollector : MonoBehaviour
{
    [SerializeField] private bool _isEndlessCollector = false;
    [SerializeField] private int _needMoneyCount = 10;

    private IEnumerator _collector;

    public event UnityAction CollectorFull;

    public void ChangeNeedMoneyCount(int needMoney)
    {
        _needMoneyCount = needMoney;
    }

    private IEnumerator CollectMoney()
    {
        while (true)
        {            
            if (Money.TryGetMoney(1))
            {
                _needMoneyCount--;
                if (_needMoneyCount == 0)
                {
                    CollectorFull?.Invoke();
                    yield break;
                }
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield break;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((_needMoneyCount <= 0) && (_isEndlessCollector == false))
            return;

        if (collider.TryGetComponent(typeof(RubbishMagnet), out Component component))
        {
            if (_collector != null)
                return;

            _collector = CollectMoney();
            StartCoroutine(_collector);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(typeof(RubbishIndicator), out Component component))
        {
            if (_collector == null)
                return;
            StopCoroutine(_collector);
            _collector = null;
        }
    }
}
