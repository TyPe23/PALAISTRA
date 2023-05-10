using Cinemachine;
using HutongGames.PlayMaker.Actions;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.UI.Image;
using state = enemyStates;

public enum enemyStates
{
    MOVE,
    HIT,
    DEATH,
    SPAWN,
    GRABBED,
    THROWN,
    PRIME,
    ATTACK,
}

public class Enemy : MonoBehaviour
{
    #region FILEDS & PROPERTIES
    public float radius = 10;
    public int health = 10;
    public float speed = 2;
    public float attackSpeed = 20;
    public float attackDist = 5;

    public bool playerDetected;
    public state state;
    public state prevState;
    public Transform parent;
    public GameObject player;
    public Transform throwRef;
    public Transform attachPoint;

    private CapsuleCollider capCollider;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private MeshRenderer mesh;

    private PlayerStates states;
    private ThirdPersonController charCon;

    private bool grounded = true;
    private bool checkingDist = false;
    private bool canGrab = true;

    private Slider slider;
    private CinemachineImpulseSource shake;

    private Dictionary<state, Action> statesStayMeths;
    private Dictionary<state, Action> statesEnterMeths;
    private Dictionary<state, Action> statesExitMeths;
    #endregion


    #region LifeCycle
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        throwRef = GameObject.FindWithTag("throwRef").transform;
        attachPoint = GameObject.FindWithTag("lariatAttach").transform;

        rb = GetComponent<Rigidbody>();
        charCon = player.GetComponent<ThirdPersonController>();
        capCollider = player.GetComponent<CapsuleCollider>();
        mesh = GetComponent<MeshRenderer>();

        states = player.GetComponent<PlayerStates>();
        shake = player.GetComponent<CinemachineImpulseSource>();
        slider = gameObject.GetComponentInChildren<Slider>();

        statesStayMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateStayMove},
            {state.HIT, StateStayHit},
            {state.DEATH, StateStaySpawn},
            {state.SPAWN, StateStayIdle},
            {state.GRABBED, StateStayGrabbed},
            {state.THROWN, StateStayThrown},
            {state.PRIME, StateStayPrime},
            {state.ATTACK, StateStayAttack},
        };

        statesEnterMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateEnterMove},
            {state.HIT, StateEnterHit},
            {state.DEATH, StateEnterDeath},
            {state.SPAWN, StateEnterSpawn},
            {state.GRABBED, StateEnterGrabbed},
            {state.THROWN, StateEnterThrown},
            {state.PRIME, StateEnterPrime},
            {state.ATTACK, StateEnterAttack},
        };

        statesExitMeths = new Dictionary<state, Action>()
        {
            {state.MOVE, StateExitMove},
            {state.HIT, StateExitHit},
            {state.DEATH, StateExitDeath},
            {state.SPAWN, StateExitSpawn},
            {state.GRABBED, StateExitGrabbed},
            {state.THROWN, StateExitThrown},
            {state.PRIME, StateExitPrime},
            {state.ATTACK, StateExitAttack},
        };

        state = state.SPAWN;
        StateEnterSpawn();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("hitbox") && gameObject.CompareTag("enemy") && canGrab)
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            ChangeState(state.GRABBED);
        }
        if (collision.transform.CompareTag("floor") && !grounded)
        {
            grounded = true;
        }
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();

        if (states.letGo && state == state.GRABBED)
        {
            ChangeState(state.THROWN);
        }

        slider.value = health;
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
    private void StateEnterPrime()
    {
        agent.speed = 0;
        StartCoroutine(charge());
    }

    private void StateEnterAttack()
    {
        canGrab = false;
        mesh.material.color = new Color(255, 0, 0);
        agent.speed = attackSpeed;
        agent.acceleration *= 10;
        agent.SetDestination(player.transform.position);
    }

    private void StateEnterThrown()
    {
        StartCoroutine(pauseCollision());

        gameObject.tag = "Untagged";

        charCon.grab = false;
        grounded = false;
        states.letGo = false;
        rb.isKinematic = false;
        transform.parent = parent;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.useGravity = true;

        mesh.material.color = new Color(0, 0, 255);
        shake.GenerateImpulseWithForce(0.1f);

        Vector3 launch = throwRef.position - player.transform.position;
        launch.y = 0.2f;

        if (states.prevState == playerStates.SPIN)
        {
            rb.AddForce(launch * 40, ForceMode.Impulse);
            health -= 2;
        }
        else if (states.prevState == playerStates.LARIAT)
        {
            rb.AddForce(launch * 30, ForceMode.Impulse);
            health -= 3;
        }
        else if (states.prevState == playerStates.PILEDRIVER)
        {
            rb.AddForce(launch * 30, ForceMode.Impulse);
            health -= 5;
        }
    }

    private void StateEnterGrabbed()
    {
        agent.speed = 0;
        agent.enabled = false;
        charCon.grab = true;
        transform.parent = attachPoint;
        transform.position = attachPoint.position;
        rb.useGravity = false;
        rb.isKinematic = true;
        mesh.material.color = new Color(255, 255, 255);
    }

    private void StateEnterSpawn()
    {
        StartCoroutine(spawn());
    }

    private void StateEnterDeath()
    {
        //trigger death anim
        mesh.material.color = new Color(0, 0, 0);
        gameObject.tag = "Untagged";
        slider.gameObject.SetActive(false);
        rb.isKinematic = true;
        capCollider.enabled = false;
        rb.detectCollisions = false;
    }

    private void StateEnterHit()
    {
    }

    private void StateEnterMove()
    {
        mesh.material.color = new Color(255, 0, 255);
        agent.speed = speed;
    }
    #endregion

    #region Stay
    private void StateStayAttack()
    {
        if ((agent.destination - transform.position).magnitude <= 0.5f)
        {
            canGrab = true;

            ChangeState(state.MOVE);
        }
    }

    private void StateStayPrime()
    {
        mesh.material.color = new Color(255, 255, 0);
        agent.SetDestination(player.transform.position);
    }

    private void StateStayThrown()
    {
        if (grounded)
        {
            if (health <= 0)
            {
                ChangeState(state.DEATH);
            }
            else
            {
                ChangeState(state.MOVE);
            }
        }
    }
    
    private void StateStayGrabbed()
    {
        agent.SetDestination(transform.position);
    }
    private void StateStayIdle()
    {
    }
    private void StateStaySpawn()
    {
    }
    private void StateStayHit()
    {
    }

    private void StateStayMove()
    {
        agent.SetDestination(player.transform.position);

        if ((transform.position - player.transform.position).magnitude <= attackDist && !checkingDist)
        {
            StartCoroutine(prime());
        }
    }
    #endregion

    #region Exit
    private void StateExitPrime()
    {
        agent.speed = speed;
    }

    private void StateExitAttack()
    {
        canGrab = true;
        agent.speed = speed;
        agent.acceleration /= 10;
    }

    private void StateExitThrown()
    {
        gameObject.tag = "enemy";
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        agent.speed = speed;
        agent.enabled = true;
    }

    private void StateExitGrabbed()
    {
    }

    private void StateExitDeath()
    {
    }
    
    private void StateExitSpawn()
    {
    }

    private void StateExitHit()
    {
    }

    private void StateExitMove()
    {
        agent.SetDestination(transform.position);
    }
    #endregion

    #region HELPER
    private IEnumerator pauseCollision()
    {
        capCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        capCollider.enabled = true;
    }

    private IEnumerator charge()
    {
        yield return new WaitForSeconds(3);
        if (state == state.PRIME)
        {
            ChangeState(state.ATTACK);
        }
    }
    private IEnumerator prime()
    {
        checkingDist = true;
        yield return new WaitForSeconds(2f);
        if (state == state.MOVE && (transform.position - player.transform.position).magnitude <= attackDist)
        {
            ChangeState(state.PRIME);
        }
        checkingDist = false;
    }
    private IEnumerator spawn()
    {
        yield return new WaitForSeconds(3f);
        ChangeState(state.MOVE);
    }
    #endregion
}