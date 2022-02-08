using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Locker> _lockers = new List<Locker>();
    [SerializeField] private int _id = 0;

    public int ID => _id;
    public bool IsUnlock { get; private set; }

    private IEnumerator StartAds()
    {
        yield return new WaitForSeconds(1);
        AddAfteLevel.Show();
    }

    public void ChangeID(int newID)
    {
        _id = newID;
    }

    public void Unlock(bool startAds)
    {
        if (IsUnlock)
            return;
        if (_lockers.Count > 0)
        {
            foreach (Locker locker in _lockers)
            {
                locker.Unlock();
            }            
        }
        IsUnlock = true;
        if (startAds)
        {
            StartCoroutine(StartAds());
        }
    }
}
