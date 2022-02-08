using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbisGroup : MonoBehaviour
{
    [SerializeField] private List<Rubbish> _templates = new List<Rubbish>();
    [SerializeField] private int _rubbishCount = 10;
    [SerializeField] private int _maxActiveRubbish = 10;
    private int _activeRubbishCount = 0;
    private List<Rubbish> _rubbishes = new List<Rubbish>();
    private List<Rubbish> _activeRubbishes = new List<Rubbish>();
    public int GroupSize => _rubbishes.Count;

    private void Init()
    {
        int rubbishCount = _rubbishCount;
        while (rubbishCount > 0)
        {
            foreach (var rubbish in _templates)
            {
                Rubbish instRubbish = Instantiate(rubbish, transform.position, Quaternion.identity);
                if (_activeRubbishCount >= _maxActiveRubbish)
                {
                    instRubbish.gameObject.SetActive(false);
                    _rubbishes.Add(instRubbish);
                }
                else
                {
                    _activeRubbishCount++;
                }
                _activeRubbishes.Add(instRubbish);
                instRubbish.Magneted += SetRubbishActive;
                rubbishCount--;
            }
        }
    }

    private void RemoveRubbishEvents()
    {
        foreach (var item in _activeRubbishes)
        {
            if (item != null)
                item.Magneted -= SetRubbishActive;
        }
    }

    private void SetRubbishActive()
    {
        foreach (var item in _rubbishes)
        {
            if (item.gameObject.activeInHierarchy == false)
            {
                item.gameObject.SetActive(true);
                _rubbishes.Remove(item);
                return;
            }
        }
        RemoveRubbishEvents();
        Destroy(gameObject);
    }

    private void Start()
    {
        Init();
    }
}
