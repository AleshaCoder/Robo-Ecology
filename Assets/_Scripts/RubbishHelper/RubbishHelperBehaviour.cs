using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishHelperBehaviour
{
    private RubbishHelperState _currentState = null;

    public void ChangeCurrentStateTo(RubbishHelperState state)
    {
        if (state == null)
        {
            Debug.LogError("Error in RubbishHelperBehaviour::ChangeCurrentStateTo -> Need init state");
            return;
        }
        if (_currentState != null)
            _currentState.Stop();
        _currentState = state;
        _currentState.Start();
    }    
}
