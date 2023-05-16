using Cinemachine;
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
    public bool canDamage = true;
    public bool letGo = false;
    public bool grab = false;
    public float launchAmount;
    public bool invul;
    public bool showScore = true;
    public Animator enemyAnim;
    public EndOfGame EOG;

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
    private Vector3 startPos;
    private bool exitDash;
    private bool canMove;
    private bool canAction = true;
    public float PDTime = 2;
    public int enemyHealth;

    public GameObject cracks;

    public bool exitPD { get; private set; }
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
            animator.SetBool("Spin", false);
            animator.SetBool("IsSpinning", false);
            
            if (enemyAnim != null)
            {
                enemyAnim.SetBool("Spin", false);
                enemyAnim.SetBool("IsSpinning", false);
            }
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

    #endregion

    #region Enter
    private void StateEnterExhausted()
    {
        letGo = true;
        //animator.SetBool();
    }

    private void StateEnterStop()
    {
        
    }
    
    private void StateEnterIdle()
    {
    }

    private void StateEnterPileDriver()
    {
        animator.SetTrigger("Piledriver");


        if (enemyAnim != null)
        {
            enemyAnim.SetBool("GetUp", false);
            enemyAnim.SetTrigger("Piledriver");
        }

        canAction = false;
        launchAmount = 15;
        stamina.spendStamina(playerStats.PileDriverCost);
        canAction = false;
        canCheck = false;
        letGo = false;
        StartCoroutine(waitToCheckGround());
        shake.GenerateImpulseWithForce(0.1f);
        momentum.addMomentum(50);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.GRAB, soundSrc);

        StartCoroutine(pileDriverAnim());
    }

    private void StateEnterSpin()
    {
        animator.SetBool("IsSpinning", true);
        animator.SetBool("Spin", true);

        if (enemyAnim != null)
        {
            enemyAnim.SetBool("IsSpinning", true);
            enemyAnim.SetBool("Spin", true);
        }

        launchAmount = 15;
        canAction = false;
        stamina.spendStamina(playerStats.SpinCost);
        canAction = false;
        letGo = false;
        shake.GenerateImpulseWithForce(0.1f);
        momentum.addMomentum(30);
        StartCoroutine(waitToCheckGround());
        Game.globalInstance.sndPlayer.PlaySound(SoundType.GRAB, soundSrc);
    }

    private void StateEnterLariat()
    {
        animator.SetBool("Lariat", true);
        if (enemyAnim != null)
        {
            enemyAnim.SetBool("GetUp", false);
            enemyAnim.SetBool("Lariat", true);
        }

        launchAmount = 0;
        canAction = false;
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
        //animator.SetTrigger();
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
        StartCoroutine(gameOverDelay());
    }

    private void StateEnterHit()
    {
        //start animation
        animator.SetBool("Hit", true);
        StartCoroutine(waitToMove());
    }

    private void StateEnterMove()
    {
        grab = false;
        letGo = true;
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
        if (canAction)
        {
            if (inputs.lariat && stamina.stamina >= playerStats.LariatCost + 5)
            {
                hitbox.enabled = true;

                if (grab)
                {
                    animator.SetTrigger("Grapple");

                    if (enemyAnim != null)
                    {
                        enemyAnim.SetTrigger("Grapple");
                    }
                    ChangeState(state.LARIAT);
                }

                StartCoroutine(HBTimeout());
            }
            else if (inputs.spin && stamina.stamina >= playerStats.SpinCost + 5)
            {
                hitbox.enabled = true;

                if (grab)
                {
                    animator.SetTrigger("Grapple");

                    if (enemyAnim != null)
                    {
                        enemyAnim.SetTrigger("Grapple");
                    }

                    ChangeState(state.SPIN);
                }

                StartCoroutine(HBTimeout());
            }
            else if (inputs.pileDriver && stamina.stamina >= playerStats.PileDriverCost + 5)
            {
                hitbox.enabled = true;

                if (grab)
                {
                    animator.SetTrigger("Grapple");

                    if (enemyAnim != null)
                    {
                        enemyAnim.SetTrigger("Grapple");
                    }

                    ChangeState(state.PILEDRIVER);
                }

                StartCoroutine(HBTimeout());
            }

            if (exitDash && state == state.DASH)
            {
                letGo = true;
                ChangeState(state.MOVE);
            }
        }
    }

    private void StateStayIdle()
    {
    }

    private void StateStayPileDriver()
    {
        if (exitPD)
        {
            ChangeState(state.MOVE);
        }
    }

    private void StateStaySpin()
    {
        stamina.spendStamina(playerStats.SpinHoldCost);
        charCon.Spin();

        if (launchAmount <= 80)
        {
            launchAmount += Time.fixedDeltaTime * 20;
        }

        if (!inputs.spin || stamina.stamina <= 1 && canCheck)
        {
            animator.SetBool("IsSpinning", false);

            if (enemyAnim != null)
            {
                enemyAnim.SetBool("IsSpinning", false);
            }
        }
        else
        {
            animator.SetBool("IsSpinning", true);

            if (enemyAnim != null)
            {
                enemyAnim.SetBool("IsSpinning", true);
            }
        }

        momentum.addMomentum(0.25f);
    }

    private void StateStayLariat()
    {

        if (canCheck && charCon.Grounded)
        {
            StartCoroutine(lariatExit());
        }
        else
        {
            charCon.Lariat();
        }
    }

    private void StateStayDash()
    {
        bool changed = false;

        charCon.Dash();

        Vector3 currPos = transform.position;

        if ((currPos - startPos).magnitude > playerStats.DashDist)
        {
            ChangeState(state.MOVE);
            changed = true;
        }

        if (canAction && !changed)
        {
            if (inputs.lariat && stamina.stamina >= playerStats.LariatCost + 5)
            {
                hitbox.enabled = true;

                if (grab)
                {
                    animator.SetTrigger("Grapple");

                    if (enemyAnim != null)
                    {
                        enemyAnim.SetTrigger("Grapple");
                    }

                    ChangeState(state.LARIAT);
                }

                StartCoroutine(HBTimeout());
            }
            else if (inputs.spin && stamina.stamina >= playerStats.SpinCost + 5)
            {
                hitbox.enabled = true;

                if (grab)
                {
                    animator.SetTrigger("Grapple");

                    if (enemyAnim != null)
                    {
                        enemyAnim.SetTrigger("Grapple");
                    }
                    ChangeState(state.SPIN);
                }

                StartCoroutine(HBTimeout());
            }
            else if (inputs.pileDriver && stamina.stamina >= playerStats.PileDriverCost + 5)
            {
                hitbox.enabled = true;

                if (grab)
                {
                    animator.SetTrigger("Grapple");

                    if (enemyAnim != null)
                    {
                        enemyAnim.SetTrigger("Grapple");
                    }
                    ChangeState(state.PILEDRIVER);
                }

                StartCoroutine(HBTimeout());
            }

            if (exitDash)
            {
                ChangeState(state.MOVE);
            }
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

        if (canAction)
        {
            if (inputs.sprint && stamina.stamina >= playerStats.DashCost)
            {
                ChangeState(state.DASH);
            }
            else
            {
                if (inputs.lariat && stamina.stamina >= playerStats.LariatCost + 5)
                {
                    hitbox.enabled = true;

                    if (grab)
                    {
                        animator.SetTrigger("Grapple");

                        if (enemyAnim != null)
                        {
                            enemyAnim.SetTrigger("Grapple");
                        }

                        ChangeState(state.LARIAT);
                    }

                    StartCoroutine(HBTimeout());
                }
                else if (inputs.spin && stamina.stamina >= playerStats.SpinCost + 5)
                {
                    hitbox.enabled = true;

                    if (grab)
                    {
                        animator.SetTrigger("Grapple");

                        if (enemyAnim != null)
                        {
                            enemyAnim.SetTrigger("Grapple");
                        }

                        ChangeState(state.SPIN);
                    }

                    StartCoroutine(HBTimeout());
                }
                else if (inputs.pileDriver && stamina.stamina >= playerStats.PileDriverCost + 5)
                {
                    hitbox.enabled = true;

                    if (grab)
                    {
                        animator.SetTrigger("Grapple");

                        if (enemyAnim != null)
                        {
                            enemyAnim.SetTrigger("Grapple");
                        }

                        ChangeState(state.PILEDRIVER);
                    }

                    StartCoroutine(HBTimeout());
                }
            }
        }
    }
    #endregion

    #region Exit
    private void StateExitExhausted()
    {
        momentum.recovery = playerStats.staminaRecovery;
        //animator.SetBool();
    }

    private void StateExitStop()
    {
    }

    private void StateExitIdle()
    {
    }

    private void StateExitPileDriver()
    {
        animator.SetBool("Grapple", false);
        animator.SetBool("Piledriver", false);

        Instantiate(cracks, transform.position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));

        if (enemyAnim != null)
        {
            enemyAnim.SetBool("Grapple", false);
            enemyAnim.SetBool("Piledriver", false);
        }

        letGo = true;
        shake.GenerateImpulseWithForce(0.5f);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.IMPACT1, soundSrc);
        StartCoroutine(IFrames());
        StartCoroutine(ActionTimeout());
    }

    private void StateExitSpin()
    {

        animator.SetBool("Grapple", false);

        if (enemyAnim != null)
        {
            enemyAnim.SetBool("Grapple", false);
        }

        animator.SetBool("IsSpinning", false);
        animator.SetBool("Spin", false);

        if (enemyAnim != null)
        {
            enemyAnim.SetBool("IsSpinning", false);
            enemyAnim.SetBool("Spin", false);
        }

        //enemyAnim.SetBool("GetUp", true);
        letGo = true;
        shake.GenerateImpulseWithForce(0.25f);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.THROW, soundSrc);
        StartCoroutine(IFrames());
        StartCoroutine(ActionTimeout());
    }

    private void StateExitLariat()
    {
        animator.SetBool("Grapple", false);
        animator.SetBool("Lariat", false);

        Instantiate(cracks, transform.position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));

        if (enemyAnim != null)
        {
            enemyAnim.SetBool("Grapple", false);
            enemyAnim.SetBool("Lariat", false);
            if (enemyHealth > 0)
            {
                enemyAnim.SetBool("GetUp", true);
            }
        }
        letGo = true;
        shake.GenerateImpulseWithForce(0.5f);
        Game.globalInstance.sndPlayer.PlaySound(SoundType.IMPACT1, soundSrc);
        StartCoroutine(IFrames()); 
        StartCoroutine(ActionTimeout());
    }

    private void StateExitDash()
    {
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
    private IEnumerator waitToCheckGround()
    {
        canCheck = false;
        yield return new WaitForSeconds(0.75f);
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

    private IEnumerator gameOverDelay()
    {
        yield return new WaitForSeconds(3);
        EOG.endGame();
    }

    private IEnumerator HBTimeout()
    {
        yield return new WaitForSeconds(0.1f);
        hitbox.enabled = false;
    }
    
    private IEnumerator lariatExit()
    {
        yield return new WaitForSeconds(0.25f);
        ChangeState(state.MOVE);
    }
    
    private IEnumerator DashExit()
    {
        exitDash = false;
        yield return new WaitForSeconds(0.25f);
        exitDash = true;
    }

    private IEnumerator ActionTimeout()
    {
        yield return new WaitForSeconds(0.25f);
        letGo = false;

        yield return new WaitForSeconds(0.75f);
        canAction = true;
    }

    private IEnumerator IFrames()
    {
        invul = true;
        yield return new WaitForSeconds(1f);
        invul = false;
    }

    private IEnumerator pileDriverAnim()
    {
        exitPD = false;
        yield return new WaitForSeconds(PDTime);
        exitPD = true;
    }
    #endregion
}