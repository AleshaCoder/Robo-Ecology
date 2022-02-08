using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishHelper : MonoBehaviour
{
    [SerializeField] private Path _walking, _toHome;
    [SerializeField] private PathMovement _pathMovement;

    [SerializeField] private RubbishMagnet _magnet;
    private IEnumerator _wait;

    private RubbishHelperMovementState _walk, _goHome;
    private RubbishHelperBehaviour _behaviour = new RubbishHelperBehaviour();

    public void SetPaths(Path walking, Path toHome)
    {
        _walking = walking;
        _toHome = toHome;
    }

    private void TryGoHome(int magnetedCount)
    {
        
        if (_magnet.MaxRubbishCount <=magnetedCount)
        {
            _behaviour.ChangeCurrentStateTo(_goHome);
        }
    }

    private void TryWalking(int magnetedCount)
    {
        if (magnetedCount == 0)
        {            
            _behaviour.ChangeCurrentStateTo(_walk);
        }
    }
    private void Walking()
    {
        _behaviour.ChangeCurrentStateTo(_goHome);
    }

    private IEnumerator WaitPaths()
    {
        while ((_walking == null) || (_toHome == null))
            yield return new WaitForFixedUpdate();

        _walk = new RubbishHelperMovementState(_pathMovement, _walking, 30);
        _goHome = new RubbishHelperMovementState(_pathMovement, _toHome, 10);

        _walk.LifeEnd += Walking;
        _behaviour.ChangeCurrentStateTo(_walk);

        _magnet.RubbishCountChanged += TryGoHome;
        _magnet.RubbishCountChanged += TryWalking;
    }

    private void OnEnable()
    {
        _wait = WaitPaths();
        StartCoroutine(_wait);
    }

    private void OnDisable()
    {
        _magnet.RubbishCountChanged -= TryGoHome;
        _magnet.RubbishCountChanged -= TryWalking;
    }
}
