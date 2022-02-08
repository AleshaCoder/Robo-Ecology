using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class PathMovement : MonoBehaviour
{
    [SerializeField] private NavMeshPath _np;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Path _path = null;
    [SerializeField] private bool _canMoving = false;

    public bool CanMove => _canMoving;

    public Path GetPath()
    {
        return _path;
    }

    public void SetPath(Path path)
    {
        _path = path;
    }

    public void StartMoving() => _canMoving = true;
    public void StopMoving() => _canMoving = false;

    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        if (_canMoving == true)
        {
            if (_path == null)
                return;
            _agent.SetDestination(_path.CurrentPoint);
        }
    }

    private void FixedUpdate()
    {
        if (_path == null)
            return;
        if (_canMoving == false)
            return;
        if (_agent.hasPath == false)
        {
            _agent.SetDestination(_path.NextPoint);
        }
    }
}
