using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using state = puzzleStates;

public enum puzzleStates
{
    RESET,
    FIRST,
    SECOND,
    THIRD,
    SOLVED,
}


public class Puzzle : MonoBehaviour
{
    public pressurePlate[] pressurePlates;
    public ProjectileSpawn[] spawners;

    private Tuple<int, int, int>[] orders = 
    { 
        new (0, 1, 2),
        new (0, 2, 1),
        new (1, 0, 2),
        new (1, 2, 0),
        new (2, 0, 1),
        new (2, 1, 0),
    };

    public Tuple<int, int, int> puzzleOrder;

    private pressurePlate first;
    private pressurePlate second;
    private pressurePlate third;

    public puzzleStates state;
    public puzzleStates prevState;

    private Dictionary<state, Action> statesStayMeths;
    private Dictionary<state, Action> statesEnterMeths;
    private Dictionary<state, Action> statesExitMeths;

    // Start is called before the first frame update
    void Start()
    {
        statesEnterMeths = new Dictionary<state, Action>()
        {
            {state.RESET, StateEnterReset},
            {state.FIRST, StateEnterFirst},
            {state.SECOND, StateEnterSecond},
            {state.THIRD, StateEnterThird},
            {state.SOLVED, StateEnterSolved},
        };

        statesStayMeths = new Dictionary<state, Action>()
        {
            {state.RESET, StateStayReset},
            {state.FIRST, StateStayFirst},
            {state.SECOND, StateStaySecond},
            {state.THIRD, StateStayThird},
            {state.SOLVED, StateStaySolved},
        };

        statesExitMeths = new Dictionary<state, Action>()
        {
            {state.RESET, StateExitReset},
            {state.FIRST, StateExitFirst},
            {state.SECOND, StateExitSecond},
            {state.THIRD, StateExitThird},
            {state.SOLVED, StateExitSolved},
        };

        state = state.RESET;
        StateEnterReset();

        puzzleOrder = orders[UnityEngine.Random.Range(0, orders.Length)];
        first = pressurePlates[puzzleOrder.Item1];
        second = pressurePlates[puzzleOrder.Item2];
        third = pressurePlates[puzzleOrder.Item3];
    }


    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();
    }

    public void ChangeState(state newState)
    {
        if (state != newState)
        {
            statesExitMeths[state].Invoke();
            prevState = state;
            state = newState;
            statesEnterMeths[state].Invoke();
        }
    }

    #region ENTER
    private void StateEnterSolved()
    {
        foreach (ProjectileSpawn spawner in spawners)
        {
            spawner.continuous = false;
        }
    }

    private void StateEnterThird()
    {
    }

    private void StateEnterSecond()
    {
    }

    private void StateEnterFirst()
    {
    }

    private void StateEnterReset()
    {
        foreach (pressurePlate plate in pressurePlates)
        {
            plate.active = false;
        }
    }
    #endregion

    #region STAY
    private void StateStaySolved()
    {
    }

    private void StateStayThird()
    {
        if (third.activated)
        {
            ChangeState(state.SOLVED);
        }
    }

    private void StateStaySecond()
    {
        if (second.activated)
        {
            ChangeState(state.THIRD);
        }
        else if (third.activated)
        {
            ChangeState(state.RESET);
        }
    }

    private void StateStayFirst()
    {
        if (first.activated)
        {
            ChangeState(state.SECOND);
        }
        else if (third.activated || second.activated)
        {
            ChangeState(state.RESET);
        }
    }

    private void StateStayReset()
    {
        ChangeState(state.FIRST);
    }
    #endregion

    #region EXIT
    private void StateExitSolved()
    {
    }

    private void StateExitThird()
    {
    }

    private void StateExitSecond()
    {
    }

    private void StateExitFirst()
    {
    }

    private void StateExitReset()
    {
    }
    #endregion
}
