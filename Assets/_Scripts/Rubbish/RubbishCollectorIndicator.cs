using UnityEngine;

public class RubbishCollectorIndicator : MonoBehaviour
{
    public delegate void IndicatingHandler();
    public event IndicatingHandler RubbishCollectorHasFound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(typeof(RubbishCollector), out Component component))
        {
            RubbishCollectorHasFound?.Invoke();
        }
    }
}
