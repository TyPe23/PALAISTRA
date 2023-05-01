using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using state = playerStates;

public enum playerStates
{
    MOVE,
    DASH,
    LARIAT,
    SPIN,
    PILEDRIVER,
    HIT,
    LOSE,
    IDLE,
}

public class PlayerStates : MonoBehaviour
{
    #region FILEDS & PROPERTIES
    public state state;
    public BoxCollider hitbox;
    public bool canCheck;
    public bool letGo = false;

    private Dictionary<playerStates, Action> statesStayMeths;
    private Dictionary<playerStates, Action> statesEnterMeths;
    private Dictionary<playerStates, Action> statesExitMeths;

    private ThirdPersonController charCon;
    private PlayerStats playerStats;
    private StarterAssetsInputs inputs;
    private StaminaManager stamina;

    private bool canAction;
    private Vector3 startPos;
    #endregion

    #region LifeCycle
    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<ThirdPersonController>();
        playerStats = GetComponent<PlayerStats>();
        inputs = GetComponent<StarterAssetsInputs>();
        stamina = GetComponent<StaminaManager>();

        statesStayMeths = new Dictionary<playerStates, Action>()
        {
            {state.MOVE, StateStayMove},
            {state.DASH, StateStayDash},
            {state.LARIAT, StateStayLariat},
            {state.SPIN, StateStaySpin},
            {state.PILEDRIVER, StateStayPileDriver},
            {state.HIT, StateStayHit},
            {state.LOSE, StateStayLose},
            {state.IDLE, StateStayIdle},
        };

        statesEnterMeths = new Dictionary<playerStates, Action>()
        {
            {state.MOVE, StateEnterMove},
            {state.DASH, StateEnterDash},
            {state.LARIAT, StateEnterLariat},
            {state.SPIN, StateEnterSpin},
            {state.PILEDRIVER, StateEnterPileDriver},
            {state.HIT, StateEnterHit},
            {state.LOSE, StateEnterLose},
            {state.IDLE, StateEnterIdle},
        };

        statesExitMeths = new Dictionary<playerStates, Action>()
        {
            {state.MOVE, StateExitMove},
            {state.DASH, StateExitDash},
            {state.LARIAT, StateExitLariat},
            {state.SPIN, StateExitSpin},
            {state.PILEDRIVER, StateExitPileDriver},
            {state.HIT, StateExitHit},
            {state.LOSE, StateExitLose},
            {state.IDLE, StateExitIdle},
        };

        state = state.MOVE;
        StateEnterMove();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();
    }

    public void ChangeState(state newState)
    {
        if (state != newState)
        {
            statesExitMeths[state].Invoke();
            state = newState;
            print(state);
            statesEnterMeths[state].Invoke();
        }

    }
    #endregion

    #region Enter
    private void StateEnterIdle()
    {
        throw new NotImplementedException();
    }

    private void StateEnterPileDriver()
    {
        stamina.spendStamina(playerStats.PileDriverCost);
        canAction = false;
        canCheck = false;
        letGo = false;
        StartCoroutine(waitToCheckGround());
    }

    private void StateEnterSpin()
    {
        stamina.spendStamina(playerStats.SpinCost);
        canAction = false;
        letGo = false;
    }

    private void StateEnterLariat()
    {
        stamina.spendStamina(playerStats.LariatCost);
        canAction = false;
        canCheck = false;
        letGo = false;
        StartCoroutine(waitToCheckGround());
    }

    private void StateEnterDash()
    {
        startPos = transform.position;
        stamina.spendStamina(playerStats.DashCost);
        canAction = true;
        inputs.sprint = false;
        inputs.pileDriver = false;
    }

    private void StateEnterLose()
    {
        throw new NotImplementedException();
    }

    private void StateEnterHit()
    {
        throw new NotImplementedException();
    }

    private void StateEnterMove()
    {
        charCon.grab = false;
    }
    #endregion

    #region Stay
    private void StateStayIdle()
    {
        throw new NotImplementedException();
    }

    private void StateStayPileDriver()
    {
        charCon.PileDriver();
        if (canCheck && charCon.Grounded)
        {
            ChangeState(state.MOVE);
        }
    }

    private void StateStaySpin()
    {
        stamina.spendStamina(playerStats.SpinHoldCost);
        charCon.Spin();

        if (!inputs.spin || stamina.stamina <= 1)
        {
            ChangeState(state.MOVE);
        }
    }

    private void StateStayLariat()
    {
        charCon.Lariat();

        if (canCheck && charCon.Grounded)
        {
            ChangeState(state.MOVE);
        }
    }

    private void StateStayDash()
    {
        charCon.Dash();

        Vector3 currPos = transform.position;

        if ((currPos - startPos).magnitude > playerStats.DashDist)
        {
            ChangeState(state.MOVE);
        }

        if (inputs.lariat && stamina.stamina >= playerStats.LariatCost)
        {
            hitbox.enabled = true;

            if (charCon.grab)
            {
                ChangeState(state.LARIAT);
            }

            StartCoroutine(HBTimeout());
        }
        else if (inputs.spin && stamina.stamina >= playerStats.SpinCost)
        {
            hitbox.enabled = true;

            if (charCon.grab)
            {
                ChangeState(state.SPIN);
            }

            StartCoroutine(HBTimeout());
        }
        else if (inputs.pileDriver && stamina.stamina >= playerStats.PileDriverCost)
        {
            hitbox.enabled = true;

            if (charCon.grab)
            {
                ChangeState(state.PILEDRIVER);
            }

            StartCoroutine(HBTimeout());
        }
    }
    private void StateStayLose()
    {
        throw new NotImplementedException();
    }

    private void StateStayHit()
    {
        throw new NotImplementedException();
    }

    private void StateStayMove()
    {
        charCon.Move();

        if (inputs.sprint)
        {
            canAction = true;
            ChangeState(state.DASH);
        }
        else if (canAction)
        {
            if (inputs.lariat && stamina.stamina >= playerStats.LariatCost)
            {
                hitbox.enabled = true;

                if (charCon.grab)
                {
                   ChangeState(state.LARIAT);
                }

                StartCoroutine(HBTimeout());
            }
            else if (inputs.spin && stamina.stamina >= playerStats.SpinCost)
            {
                hitbox.enabled = true;

                if (charCon.grab)
                {
                    ChangeState(state.SPIN);
                }

                StartCoroutine(HBTimeout());
            }
            else if (inputs.pileDriver && stamina.stamina >= playerStats.PileDriverCost)
            {
                hitbox.enabled = true;

                if (charCon.grab)
                {
                    ChangeState(state.PILEDRIVER);
                }

                StartCoroutine(HBTimeout());
            }
        }
    }
    #endregion

    #region Exit
    private void StateExitIdle()
    {
        throw new NotImplementedException();
    }

    private void StateExitPileDriver()
    {
        letGo = true;
    }

    private void StateExitSpin()
    {
        letGo = true;
    }

    private void StateExitLariat()
    {
        letGo = true;
    }

    private void StateExitDash()
    {
        StartCoroutine(canActionWait());
    }

    private void StateExitLose()
    {
        throw new NotImplementedException();
    }

    private void StateExitHit()
    {
        throw new NotImplementedException();
    }

    private void StateExitMove()
    {
    }
    #endregion

    #region helper
    private IEnumerator canActionWait()
    {
        yield return new WaitForSeconds(0.1f);
        canAction = false;
    }

    private IEnumerator waitToCheckGround()
    {
        yield return new WaitForSeconds(0.5f);
        canCheck = true;
    }

    private IEnumerator HBTimeout()
    {
        yield return new WaitForSeconds(0.1f);
        hitbox.enabled = false;
    }
    #endregion
}