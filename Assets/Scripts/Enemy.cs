using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
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

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    #region FILEDS & PROPERTIES
    public float radius = 10;
    public int health = 10;
    public float speed = 3;
    public float attackSpeed = 20;
    public float attackDist = 5;
    public float distToPlayer;
    public float prevDist;
    public bool debug;

    public state state;
    public state prevState;
    public Transform parent;
    public GameObject player;
    public Transform throwRef;
    public Transform attachPoint;
    private Transform spinPoint;

    private CapsuleCollider capCollider;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private MeshRenderer mesh;
    private Animator anim;
    public SphereCollider spinCollider;
    private AudioSource soundSrc;

    private PlayerStates states;
    private StarterAssetsInputs inputs;

    private bool grounded = true;
    private bool checkingDist = false;
    private bool canGrab = true;

    private Slider slider;
    private CinemachineImpulseSource shake;

    private Dictionary<state, Action> statesStayMeths;
    private Dictionary<state, Action> statesEnterMeths;
    private Dictionary<state, Action> statesExitMeths;
    public maintainOrientation reset;
    public bool resetPos;
    private bool canHit = true;
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
        spinPoint = GameObject.FindWithTag("spinAttach").transform;

        rb = GetComponent<Rigidbody>();
        mesh = GetComponentInChildren<MeshRenderer>();
        anim = GetComponentInChildren<Animator>();
        soundSrc = GetComponent<AudioSource>();

        capCollider = player.GetComponent<CapsuleCollider>();
        states = player.GetComponent<PlayerStates>();
        inputs = player.GetComponent<StarterAssetsInputs>();
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
        if (collision.gameObject.CompareTag("hitbox") && state != state.DEATH && states.state != playerStates.EXHAUSTED && (state != state.ATTACK || canGrab))
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            states.grab = true;
            states.letGo = false;
            states.enemyAnim = anim;
            states.enemyHealth = health;
            ChangeState(state.GRABBED);
        }
        if (collision.transform.CompareTag("floor") && !grounded)
        {
            grounded = true;
            StartCoroutine(getUp());
            Game.globalInstance.sndPlayer.PlaySound(SoundType.IMPACT1, soundSrc);
        }
        if (collision.transform.CompareTag("trap") && canGrab && state != state.DEATH)
        {
            health -= 10;
            ChangeState(state.HIT);
        }
        if (collision.transform.CompareTag("projectile") && canGrab && state != state.DEATH)
        {
            collision.transform.tag = "enemyNoHit";
            health -= 2;
            ChangeState(state.HIT);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("spunObj") && canHit && canGrab && state != state.DEATH)
        {
            health -= 2;
            ChangeState(state.HIT);
        }
    }

    private void Update()
    {
        prevDist = distToPlayer;
        distToPlayer = (transform.position - player.transform.position).magnitude;

        if (distToPlayer - prevDist > 0.1f && state != state.THROWN)
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
        }

        if (states.letGo && transform.parent != parent)
        {
            ChangeState(state.THROWN);
        }
    }


    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();

        slider.value = health;

        anim.SetFloat("Speed", agent.speed * 2);

        if (health <= 0 && state != state.DEATH)
        {
            ChangeState(state.DEATH);
        }
    }

    public void ChangeState(state newState)
    {
        if (state != newState)
        {
            reset.resetPos();
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
        anim.SetBool("Prime", true);
        agent.speed = 0.25f;
        StartCoroutine(charge());
    }

    private void StateEnterAttack()
    {
        anim.SetBool("IsCharging", true);

        canGrab = false;

        gameObject.tag = "enemy";

        if (debug)
        {
            mesh.material.color = new Color(255, 0, 0);
        }

        agent.speed = attackSpeed;
        agent.acceleration *= 10;

        if (agent.enabled)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void StateEnterThrown()
    {
        tag = "projectile";


        states.grab = false;
        grounded = false;
        states.letGo = false;
        rb.isKinematic = false;
        transform.parent = parent;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.useGravity = true;

        if (debug)
        {
            mesh.material.color = new Color(0, 0, 255);
        }
        shake.GenerateImpulseWithForce(0.1f);

        Vector3 launch = throwRef.position - player.transform.position;
        launch.y = 0.1f;

        if (states.prevState == playerStates.SPIN)
        {
            health -= 2;
        }
        else if (states.prevState == playerStates.LARIAT)
        {
            health -= 3;
        }
        else if (states.prevState == playerStates.PILEDRIVER)
        {
            health -= 5;
        }

        rb.AddForce(launch * states.launchAmount, ForceMode.Impulse);
    }

    private void StateEnterGrabbed()
    {
        tag = "spunObj";

        canGrab = false;

        spinCollider.enabled = true;

        if (agent.enabled)
        {
            agent.SetDestination(transform.position);
        }
        agent.speed = 0;
        agent.enabled = false;
        states.grab = true;

        if (inputs.spin)
        {
            transform.parent = spinPoint;
            transform.position = spinPoint.position;
        }
        else
        {
            transform.parent = attachPoint;
            transform.position = attachPoint.position;
        }

        transform.localRotation = Quaternion.Euler(0, 0, 0);

        rb.useGravity = false;
        rb.isKinematic = true;
        capCollider.isTrigger = true;
        if (debug)
        {
            mesh.material.color = new Color(255, 255, 255);
        }
    }

    private void StateEnterSpawn()
    {
        StartCoroutine(spawn());
    }

    private void StateEnterDeath()
    {
        agent.speed = 0;

        if (debug)
        {
            mesh.material.color = new Color(0, 0, 0);
        }
        
        slider.gameObject.SetActive(false);
        rb.isKinematic = true;
        capCollider.enabled = false;
        rb.detectCollisions = false;
    }

    private void StateEnterHit()
    {
        if (debug)
        {
            mesh.material.color = new Color(255, 255, 255);
        }
        if (agent.enabled)
        {
            agent.SetDestination(transform.position);
        }
        StartCoroutine(hitStun());
    }

    private void StateEnterMove()
    {
        if (debug)
        {
            mesh.material.color = new Color(255, 0, 255);
        }
        agent.speed = speed;
    }
    #endregion

    #region Stay
    private void StateStayAttack()
    {
        if ((agent.destination - transform.position).magnitude <= 1.1f)
        {
            canGrab = true;

            StartCoroutine(attackRecoil());
        }
    }

    private void StateStayPrime()
    {
        if (debug)
        {
            mesh.material.color = new Color(255, 255, 0);
        }
        if (agent.enabled)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void StateStayThrown()
    {
        reset.resetPos();

        if (states.exitPD)
        {
            if (health <= 0)
            {
                ChangeState(state.DEATH);
            }
            else
            {
                StartCoroutine(getUp());
            }
        }
    }
    
    private void StateStayGrabbed()
    {
        
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
        if (agent.enabled)
        {
            agent.SetDestination(player.transform.position);
        }

        if ((transform.position - player.transform.position).magnitude <= attackDist && !checkingDist)
        {
            StartCoroutine(prime());
        }
    }
    #endregion

    #region Exit
    private void StateExitPrime()
    {
        reset.resetPos();
        agent.speed = speed;

        anim.SetBool("Prime", false);
    }

    private void StateExitAttack()
    {
        reset.resetPos();
        anim.SetBool("IsCharging", false);
        gameObject.tag = "enemyNoHit";
        canGrab = true;
        agent.speed = speed;
        agent.acceleration /= 10;
    }

    private void StateExitThrown()
    {
        tag = "enemyNoHit";

        reset.resetPos();
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        agent.speed = speed;
        agent.enabled = true;
    }

    private void StateExitGrabbed()
    {
        anim.SetBool("Spin", false);
        anim.SetBool("IsSpinning", false);
        tag = "projectile";
        spinCollider.enabled = false;
        reset.resetPos();
        capCollider.isTrigger = false;
        transform.localPosition = new Vector3(0, 0.5f, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void StateExitDeath()
    {
        reset.resetPos();
    }
    
    private void StateExitSpawn()
    {
        reset.resetPos();
    }

    private void StateExitHit()
    {
        reset.resetPos();
    }

    private void StateExitMove()
    {
        reset.resetPos();
        if (agent.enabled)
        {
            agent.SetDestination(transform.position);
        }
    }
    #endregion

    #region HELPER
    private IEnumerator hitStun()
    {
        Vector3 dir = throwRef.position - player.transform.position;

        rb.AddForce(dir, ForceMode.Impulse);

        canHit = false;

        anim.SetBool("GetHit", true);

        yield return new WaitForSeconds(0.5f);

        anim.SetBool("GetHit", false);
        if (health <= 0)
        {
            ChangeState(state.DEATH);
        }
        else
        {
            ChangeState(state.MOVE);
        }
        yield return new WaitForSeconds(0.5f);
        canHit = true;

    }
    
    private IEnumerator attackRecoil()
    {
        gameObject.tag = "enemyNoHit";

        anim.SetBool("IsCharging", false);
        agent.speed = 0;

        if (debug)
        {
            mesh.material.color = new Color(255, 0, 255);
        }

        canGrab = true;
        yield return new WaitForSeconds(1f);

        if (state == state.ATTACK)
        {
            ChangeState(state.MOVE);
        }
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
        if (state == state.MOVE && (transform.position - player.transform.position).magnitude <= attackDist && state == state.MOVE)
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
    private IEnumerator getUp()
    {
        reset.resetPos();
        yield return new WaitForSeconds(1f);

        if (health > 0)
        {
            anim.SetBool("GetUp", true);
        }

        yield return new WaitForSeconds(1.5f);
        canGrab = true;
        ChangeState(state.MOVE);
    }
    #endregion
}
