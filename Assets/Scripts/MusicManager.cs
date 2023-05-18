using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.ParticleSystem;
using state = musicStates;

public enum musicStates
{
    IN_COMBAT,
    OUT_COMBAT,
    START_COMBAT,
}

public class MusicManager : MonoBehaviour
{
    public AudioSource audioBase;
    public AudioSource audioCombat;
    public AudioSource audioLoop;
    public AudioSource finishBell;
    public AudioSource startBell;

    public Enemy[] enemies;
    public bool inCombat = false; 
    public bool stopCombatTrigger = false; 
    public musicStates state;
    public musicStates prevState;

    private Dictionary<state, Action> statesStayMeths;
    private Dictionary<state, Action> statesEnterMeths;
    private Dictionary<state, Action> statesExitMeths;

    // Start is called before the first frame update
    void Start()
    {
        statesEnterMeths = new Dictionary<state, Action>()
        {
            {state.OUT_COMBAT, StateEnterOOC},
            {state.IN_COMBAT, StateEnterCombat},
            {state.START_COMBAT, StateEnterStart},
        };

        statesStayMeths = new Dictionary<state, Action>()
        {
            {state.OUT_COMBAT, StateStayOOC},
            {state.IN_COMBAT, StateStayCombat},
            {state.START_COMBAT, StateStayStart},
        };

        statesExitMeths = new Dictionary<state, Action>()
        {
            {state.OUT_COMBAT, StateExitOOC},
            {state.IN_COMBAT, StateExitCombat},
            {state.START_COMBAT, StateExitStart},
        };

        state = state.OUT_COMBAT;
        StateEnterOOC();
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
    private void StateEnterCombat()
    {
        audioLoop.Play();
    }

    private void StateEnterOOC()
    {
        inCombat = false;
        audioBase.Play();
    }
    private void StateEnterStart()
    {
        inCombat = true;
        stopCombatTrigger = true;
        startBell.Play();
        audioCombat.Play();
    }
    #endregion

    #region STAY
    private void StateStayCombat()
    {
        if (!audioLoop.isPlaying)
        {
            if (enemies != null)
            {
                inCombat = enemies.Any(x => x.health > 0);

                if (!inCombat)
                {
                    finishBell.Play();
                    ChangeState(state.OUT_COMBAT);
                }
                else
                {
                    audioLoop.Play();
                }
            }
            else
            {
                ChangeState(state.OUT_COMBAT);
            }
        }
    }

    private void StateStayOOC()
    {
        if (!audioBase.isPlaying)
        {
            if (enemies != null)
            {
                if ((enemies[0].state != enemyStates.SPAWN || enemies[0].state != enemyStates.DEATH) && !stopCombatTrigger)
                {
                    ChangeState(state.START_COMBAT);
                }
                else
                {
                    audioBase.Play();
                }
            }
            else
            {
                audioBase.Play();
            }
        }
    }
    private void StateStayStart()
    {
        if (!audioCombat.isPlaying)
        {
            ChangeState(state.IN_COMBAT);
        }
    }
    #endregion

    #region EXIT
    private void StateExitCombat()
    {
    }

    private void StateExitOOC()
    {
    }

    private void StateExitStart()
    {
    }
    #endregion
}
