using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RubbishPool : MonoBehaviour
{
    [SerializeField] private long _maxCount = 500;
    private List<Rubbish> _rubbish = new List<Rubbish>(500);
    private List<Rubbish> _hidenRubbish = new List<Rubbish>(500);
    private List<Rubbish> _busyRubbish = new List<Rubbish>(500);
    public bool Full => _rubbish.Count >= _maxCount;
    public bool HasHiden => _hidenRubbish.Count > 0;

    public bool[] HasType = new bool[20];

    public void AddRubbish(Rubbish rubbish)
    {
        if (Full)
            return;
        _rubbish.Add(rubbish);
        rubbish.OnHide += ReplaceInHidenList;
        rubbish.OnShow += RemoveFromHidenList;
    }

    public Rubbish GetHidenRubbish()
    {
        if (HasHiden == false)
            return null;
        Rubbish rubbish = _hidenRubbish[0];
        _hidenRubbish.Remove(rubbish);
        _busyRubbish.Add(rubbish);
        return rubbish;
    }

    public bool TryGetHidenRubbish(RubbishType rubbishType, out Rubbish target)
    {
        target = null;
        if (HasHiden == false)
            return false;
        if (HasType[(int)rubbishType] == false)
            return false;
        foreach (Rubbish rubbish in _hidenRubbish)
        {
            if (rubbish.RubbishType == rubbishType)
            {
                target = rubbish;
                return true;
            }
        }
        HasType[(int)rubbishType] = false;
        return false;
    }

    public Rubbish[] GetRubbishes()
    {
        int length = _rubbish.Count;
        Rubbish[] rubbishes = new Rubbish[length];
        Array.Copy(_rubbish.ToArray(), rubbishes, length);
        return rubbishes;
    }

    private void ReplaceInHidenList(Rubbish rubbish)
    {
        HasType[(int)rubbish.RubbishType] = true;
        _hidenRubbish.Add(rubbish);
    }
    private void RemoveFromHidenList(Rubbish rubbish)
    {
        _hidenRubbish.Remove(rubbish);
        _busyRubbish.Remove(rubbish);
    }
}
