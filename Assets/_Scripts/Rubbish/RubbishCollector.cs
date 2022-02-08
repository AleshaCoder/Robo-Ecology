using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RubbishCollector : MonoBehaviour
{
    [SerializeField] private MagnetOwner _collectorFor = MagnetOwner.Player;
    [SerializeField] private bool _isEndlessCollector = true;
    [SerializeField] private List<CollectingRubbish> _collectingRubbishes;
    
    private List<Rubbish> _nearestRubbish = new List<Rubbish>();
    private CollectingRubbish _currentRubbish;

    public bool IsFull { get; private set; }

    public event UnityAction<Rubbish> RubbishFound;
    public event UnityAction CollectorFull;

    public void ChangeCollectingRubbish(List<CollectingRubbish> collectingRubbishes)
    {
        _collectingRubbishes = collectingRubbishes;
    }

    private bool CheckFulling()
    {
        bool isFull = true;
        foreach (var item in _collectingRubbishes)
        {
            if (item.Count > 0)
            {
                isFull = false;
                break;
            }
        }
        return isFull;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsFull == true)
            return;
        if (_collectingRubbishes.Count == 0)
        {
            IsFull = CheckFulling();
            CollectorFull?.Invoke();
            return;
        }
        if (other.TryGetComponent(typeof(RubbishMagnet),out Component component))
        {
            RubbishMagnet magnet = component as RubbishMagnet;
            if (_collectorFor != magnet.Owner)
                return;            
            _nearestRubbish = magnet.GetMagnetedRubbish();
            foreach (var item in _nearestRubbish)
            {
                foreach (var collectingRubbish in _collectingRubbishes)
                {
                    if (collectingRubbish.Type == item.RubbishType)
                    {
                        if ((_isEndlessCollector == false) && (collectingRubbish.Count <= 0))
                        {
                            IsFull = CheckFulling();
                            CollectorFull?.Invoke();
                            return;
                        }
                        RubbishFound?.Invoke(item);                        
                        item.AnimJumpTo(transform.position);
                        item.MarkNextDestruction();
                        collectingRubbish.DecreaseCount();
                        break;
                    }
                }
            }
        }
    }
}
