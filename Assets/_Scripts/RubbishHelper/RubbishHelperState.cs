using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RubbishHelperState
{
    protected float LifeTime = 10f;
    public UnityAction LifeEnd;
    public abstract void Start();

    public abstract void Stop();
}

public class RubbishHelperMovementState : RubbishHelperState
{
    private PathMovement _movement;
    private Path _path;

    public RubbishHelperMovementState(PathMovement movement, Path path, float time = 10f)
    {
        LifeTime = time;
        _movement = movement;
        _path = path;
        Live();
    }
    public override void Start()
    {
        _movement.SetPath(_path);
        _movement.StartMoving();
    }

    public override void Stop()
    {
        _movement.StopMoving();
    }

    private IEnumerator Live()
    {
        yield return new WaitForSeconds(LifeTime);
        LifeEnd?.Invoke();
    }
}
