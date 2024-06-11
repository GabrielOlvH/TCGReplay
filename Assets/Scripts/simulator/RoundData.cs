using System;
using System.Collections.Generic;
using UnityEngine;

public class RoundData
{
    public int Round;
    public List<Action> Actions = new();
    public static int DelayBetweenActions = 120;

    private int _currentAction = 0;

    public RoundData(int round)
    {
        Round = round;
    }

    public RoundData Copy()
    {
        var copy = new RoundData(0);
        copy.Actions.AddRange(Actions);
        return copy;
    }

    public Action Next()
    {
        Debug.Log(_currentAction + " / " + Actions.Count);
        return Actions[_currentAction++];
    }

    public bool HasNext()
    {
        return Actions.Count > _currentAction + 1;
    }
}