using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _pool;

    public void DestroyPool()
    {
        Destroy(_pool);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_pool == null)
            return;
        if (collision.gameObject.TryGetComponent(typeof(Rubbish), out Component component))
        {
            Rubbish rubbish = component as Rubbish;
            if (rubbish.RubbishType == RubbishType.Pink)
                rubbish.transform.parent = _pool.transform;
        }
    }
}
