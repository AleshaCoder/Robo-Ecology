using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTrigger : MonoBehaviour
{
    [SerializeField] private bool _useInput = false;
    [SerializeField] private KeyCode _key = KeyCode.Q;
    [SerializeField] private int _needKeyDownCount = 1;
    [SerializeField] private UnityEvent ChangesWhenTrigger;

    private bool _playerInTriggerZone = false;
    private Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        if (_useInput == false)
            ChangesWhenTrigger?.Invoke();

        _playerInTriggerZone = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
            return;
        if (_playerInTriggerZone == true)
            return;
        if (_useInput == false)
            ChangesWhenTrigger?.Invoke();
        _playerInTriggerZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            return;

        _playerInTriggerZone = false;
    }

    private void Update()
    {
        if (_playerInTriggerZone == false)
            return;
        if (_useInput == false)
            return;
        if (Input.GetKeyDown(_key))
        {
            _needKeyDownCount -= 1;
            if (_needKeyDownCount <=0)
               ChangesWhenTrigger?.Invoke();
        }
    }
}
