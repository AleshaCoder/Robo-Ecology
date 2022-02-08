using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Locker : MonoBehaviour
{
    [SerializeField]private GameObject _text;
    [SerializeField] private UnityEvent _onUnlock;
    private Collider _lockColider;

    public void Unlock()
    {
        if (_lockColider == null)
            _lockColider = GetComponent(typeof(Collider)) as Collider;
        _lockColider.isTrigger = true;
        _text.SetActive(false);
        try
        {
            _onUnlock?.Invoke();
        }
        catch
        {

        }
    }

    private void Awake()
    {
        _lockColider = GetComponent(typeof(Collider)) as Collider;
        _lockColider.isTrigger = false;
    }
}
