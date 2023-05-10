using Cinemachine;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    STOP,
    EXHAUSTED,
}

[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(StarterAssetsInputs))]
[RequireComponent(typeof(StaminaManager))]
[RequireComponent(typeof(CinemachineImpulseSource))]
[RequireComponent(typeof(MomentumManager))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class PlayerStates : MonoBehaviour
{
    #region FILEDS & PROPERTIES
    public state state;
    public state prevState;
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
    private MomentumManager momentum;
    private AudioSource soundSrc;
    private Animator animator;
    private TMP_Text scoreUI;
    private CinemachineImpulseSource shake;



    private bool canAction;
    private Vector3 startPos;
    private bool exitDash;
    public bool invul;
    public bool showScore = true;
    private bool canMove;
    private bool canDash = true;
    #endregion

    #region LifeCycle
    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<ThirdPersonController>();
        playerStats = GetComponent<PlayerStats>();
        inputs = GetComponent<StarterAssetsInputs>();
        stamina = GetComponent<StaminaManager>();
        shake = GetComponent<CinemachineImpulseSource>();
        momentum = GetComponent<MomentumManager>();
        soundSrc = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        scoreUI = GetComponentInChildren<TMP_Text>();

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
            {state.STOP, StateStayStop},
            {state.EXHAUSTED, StateStayExhausted},
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
            {state.STOP, StateEnterStop},
            {state.EXHAUSTED, StateEnterExhausted},
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
            {state.STOP, StateExitStop},
            {state.EXHAUSTED, StateExitExhausted},
        };

        state = state.MOVE;
        StateEnterMove();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();

        if (showScore)
        {
            scoreUI.text = $"{playerStats.score - (int)Time.time} \n" +
                           $"+ {momentum.momentumScore}";
        }

        if (stamina.stamina <= 5)
        {
            ChangeState(state.EXHAUSTED);
        }
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

    private void OnTriggerStay(Collider collision)
    {
        if ((collision.transform.CompareTag("enemy") || collision.transform.CompareTag("trap") || collision.transform.CompareTag("projectile")) && !invul && (state == state.MOVE || state == state.EXHAUSTED))
        {
            ChangeState(state.HIT);

            if (collision.transform.CompareTag("enemy") || collision.transform.CompareTag("projectile"))
            {
                playerStats.takeDamage(2);
            }
            else if (collision.transform.CompareTag("trap"))
            {
                playerStats.takeDamage(5);
            }
        }
        else if (collision.transform.CompareTag("enemy") && state == state.DASH)
        {
            ChangeState(state.STOP);
        }
    }

    #endregion

    #region Enter
    private void StateEnterExhausted()
    {
        
    }

    private void StateEnterStop()
    {
        
    }
    
    private void StateEnterIdle()
    {
    }

    private void StateEnterPileDriver()
    {
        stamina.spendStamina(playerStats.PileDriverCost);
        canAction = false;
        canCheck = false;
        letGo = false;
        StartCoroutine(waitToCheckGround());
        shake.GenerateImpulseWithForce(0.1f);
        momentum.addMomentum(50);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.GRAB, soundSrc);
    }

    private void StateEnterSpin()
    {
        stamina.spendStamina(playerStats.SpinCost);
        canAction = false;
        letGo = false;
        shake.GenerateImpulseWithForce(0.1f);
        momentum.addMomentum(30);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.GRAB, soundSrc);
    }

    private void StateEnterLariat()
    {
        stamina.spendStamina(playerStats.LariatCost);
        canAction = false;
        canCheck = false;
        letGo = false;
        StartCoroutine(waitToCheckGround());
        shake.GenerateImpulseWithForce(0.1f);
        momentum.addMomentum(30);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.GRAB, soundSrc);
    }

    private void StateEnterDash()
    {
        startPos = transform.position;
        stamina.spendStamina(playerStats.DashCost);
        canAction = true;
        inputs.sprint = false;
        inputs.pileDriver = false;
        shake.GenerateImpulseWithForce(0.1f);
        momentum.addMomentum(10);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.DASH, soundSrc);

        StartCoroutine(DashExit());
    }

    private void StateEnterLose()
    {
    }

    private void StateEnterHit()
    {
        //start animation
        animator.SetBool("Hit", true);
        StartCoroutine(waitToMove());
    }

    private void StateEnterMove()
    {
        charCon.grab = false;
    }
    #endregion

    #region Stay
    private void StateStayExhausted()
    {
        momentum.recovery = playerStats.exhaustedRecovery;

        charCon.Exhausted();

        if (stamina.stamina >= 50)
        {
            ChangeState(state.MOVE);
        }
    }

    private void StateStayStop()
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

        if (exitDash)
        {
            ChangeState(state.MOVE);
        }
    }

    private void StateStayIdle()
    {
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


        momentum.addMomentum(0.25f);
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

        if (exitDash)
        {
            ChangeState(state.MOVE);
        }
    }
    private void StateStayLose()
    {
    }

    private void StateStayHit()
    {
        //wait til animation done
        if (playerStats.currentHealth <= 0)
        {
            ChangeState(state.LOSE);
        }

        if (canMove)
        {
            if (prevState == state.EXHAUSTED)
            {
                ChangeState(state.EXHAUSTED);
            }
            else
            {
                ChangeState(state.MOVE);
            }
        }
    }

    private void StateStayMove()
    {
        letGo = true;
        charCon.Move();

        if (inputs.sprint && stamina.stamina >= playerStats.DashCost && canDash)
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
    private void StateExitExhausted()
    {
        momentum.recovery = playerStats.staminaRecovery;
    }

    private void StateExitStop()
    {
    }

    private void StateExitIdle()
    {
    }

    private void StateExitPileDriver()
    {
        letGo = true;
        shake.GenerateImpulseWithForce(0.5f);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.IMPACT, soundSrc);
        StartCoroutine(IFrames());
    }

    private void StateExitSpin()
    {
        letGo = true;
        shake.GenerateImpulseWithForce(0.25f);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.IMPACT, soundSrc);
        StartCoroutine(IFrames());
    }

    private void StateExitLariat()
    {
        letGo = true;
        shake.GenerateImpulseWithForce(0.5f);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.IMPACT, soundSrc);
        StartCoroutine(IFrames());
    }

    private void StateExitDash()
    {
        StartCoroutine(canActionWait());
        StartCoroutine(DashTimeout());
    }

    private void StateExitLose()
    {
    }

    private void StateExitHit()
    {
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

    private IEnumerator waitToMove()
    {
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Hit", false);
        canMove = true;
        StartCoroutine(IFrames());
    }

    private IEnumerator HBTimeout()
    {
        yield return new WaitForSeconds(0.1f);
        hitbox.enabled = false;
    }
    
    private IEnumerator DashExit()
    {
        exitDash = false;
        yield return new WaitForSeconds(0.25f);
        exitDash = true;
    }

    private IEnumerator DashTimeout()
    {
        canDash = false;
        yield return new WaitForSeconds(0.25f);
        canDash = true;
    }

    private IEnumerator IFrames()
    {
        invul = true;
        yield return new WaitForSeconds(0.5f);
        invul = false;
    }
    #endregion
}