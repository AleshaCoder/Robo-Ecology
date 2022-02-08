using System.Collections.Generic;
using UnityEngine;

public class RubbishIndicator : MonoBehaviour
{
    public delegate void IndicatingHandler(Rubbish rubbish);
    public event IndicatingHandler RubbishHasFound;
    public event IndicatingHandler RubbishHasLost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(typeof(Rubbish), out Component component))
        {
            Rubbish r = component as Rubbish;
            if ((!r.DestroyedSoon)||(!r.Hiden))
                RubbishHasFound?.Invoke(r);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(typeof(Rubbish), out Component component))
        {
            Rubbish r = component as Rubbish;
            if ((!r.DestroyedSoon) || (!r.Hiden))
                RubbishHasLost?.Invoke(r);
        }
    }
}
