using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using state = potStates;

public enum potStates
{
    IDLE,
    GRABBED,
    THROWN,
    BREAK,
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class VasePhysics : MonoBehaviour
{
    #region FILEDS & PROPERTIES
    [Tooltip("Input the possible collidable items tags that can break")]
    [SerializeField] private List<string> collidables;
    [Tooltip("Drag prefabs to here to add them to the drop pool of a vase.")]
    [SerializeField] private List<GameObject> spawns;
    [Tooltip("Int - chance that an item will spawn from a vase - 1/(max-1)")]
    [SerializeField] private int max;

    public bool debug;
    public state state;
    public state prevState;
    public Transform throwRef;
    public Transform attachPoint;
    public GameObject vase;
    public BoxCollider trigger;
    public BoxCollider vaseCollider;
    public GameObject vasePieces;

    private MeshRenderer mesh;
    private Animator anim;
    private Rigidbody rb;
    private PlayerStates playerState;
    private AudioSource soundSrc;
    private Transform parent;
    private GameObject player;
    private StarterAssetsInputs inputs;
    private bool grounded = true;

    private Dictionary<state, Action> statesStayMeths;
    private Dictionary<state, Action> statesEnterMeths;
    private Dictionary<state, Action> statesExitMeths;
    #endregion
    
    #region LifeCycle
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        soundSrc = GetComponent<AudioSource>();
        playerState = player.GetComponent<PlayerStates>();
        inputs = player.GetComponent<StarterAssetsInputs>();
        anim = GetComponent<Animator>();

        parent = transform.parent;
        parent = transform.parent;
        
        throwRef = GameObject.FindWithTag("throwRef").transform;
        attachPoint = GameObject.FindWithTag("vaseAttach").transform;

        statesStayMeths = new Dictionary<state, Action>()
        {
            {state.IDLE, StateStayIdle},
            {state.GRABBED, StateStayGrabbed},
            {state.THROWN, StateStayThrown},
            {state.BREAK, StateStayBreak},
        };

        statesEnterMeths = new Dictionary<state, Action>()
        {
            {state.IDLE, StateEnterIdle},
            {state.GRABBED, StateEnterGrabbed},
            {state.THROWN, StateEnterThrown},
            {state.BREAK, StateEnterBreak},
        };

        statesExitMeths = new Dictionary<state, Action>()
        {
            {state.IDLE, StateExitIdle},
            {state.GRABBED, StateExitGrabbed},
            {state.THROWN, StateExitThrown},
            {state.BREAK, StateExitBreak},
        };

        state = state.IDLE;
        StateEnterIdle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        statesStayMeths[state].Invoke();

        if (playerState.letGo && state == state.GRABBED)
        {
            ChangeState(state.THROWN);
        }
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("hitbox") && inputs.spin)
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            playerState.grab = true;
            playerState.letGo = false;
            playerState.enemyAnim = anim;
            ChangeState(state.GRABBED);
        }

        if (collision.CompareTag("Player") && playerState.state == playerStates.DASH)
        {
            print(collision.name);
            ChangeState(state.BREAK);
        }
        if (state != state.GRABBED)
        {
            if (!(collision.transform.CompareTag("Player") || collision.transform.CompareTag("Untagged")) && !grounded)
            {
                print(collision.name);
                grounded = true;
            }
            if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("enemyNoHit") || collision.transform.CompareTag("trap") || collision.transform.CompareTag("terrain"))
            {
                print(collision.name);
                ChangeState(state.BREAK);
            }
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
    private void StateEnterThrown()
    {
        playerState.grab = false;
        grounded = false;
        playerState.letGo = false;
        rb.isKinematic = false;
        transform.parent = parent;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.useGravity = true;

        if (debug)
        {
            mesh.material.color = new Color(0, 0, 255);
        }

        Vector3 launch = throwRef.position - player.transform.position;
        launch.y = 0.1f;

        float launchAmount = playerState.launchAmount * 2;

        if (launchAmount > 40)
        {
            launchAmount = 40;
        }

        rb.AddForce(launch * launchAmount, ForceMode.Impulse);
    }

    private void StateEnterBreak()
    {
        Game.globalInstance.sndPlayer.PlaySound(SoundType.POT_BREAK, soundSrc);

        vasePieces.transform.parent = parent;

        vasePieces.SetActive(true);

        int chance = UnityEngine.Random.Range(1, max);
        if (chance == max - 1)
        {
            int spawnNumber = UnityEngine.Random.Range(0, spawns.Count);
            Instantiate(spawns[spawnNumber], transform.position, Quaternion.identity);
        }
        StartCoroutine(waitThenDestroy());
    }

    private void StateEnterIdle()
    {
    }

    private void StateEnterGrabbed()
    {
        playerState.grab = true;
        transform.parent = attachPoint;
        transform.position = attachPoint.position;
        rb.useGravity = false;
        rb.isKinematic = true;
        vaseCollider.enabled = false;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        tag = "spunObj";

        if (debug)
        {
            mesh.material.color = new Color(255, 255, 255);
        }
    }

    private void StateEnterSpawn()
    {

    }
    #endregion

    #region Stay
    private void StateStayBreak()
    {
    }

    private void StateStayThrown()
    {
        if (grounded)
        {
            ChangeState(state.BREAK);
        }
    }

    private void StateStayGrabbed()
    {

    }
    private void StateStayIdle()
    {

    }
    #endregion

    #region Exit
    private void StateExitBreak()
    {
    }

    private void StateExitThrown()
    {
    }

    private void StateExitGrabbed()
    {
        tag = "projectile";
        StartCoroutine(pauseCollision());
    }
    private void StateExitIdle()
    {
    }
    #endregion

    #region HELPER
    private IEnumerator waitThenDestroy()
    {
        vaseCollider.enabled = false;
        trigger.enabled = false;
        vase.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator pauseCollision()
    {
        vaseCollider.enabled = false;

        yield return new WaitForSeconds(0.05f);

        transform.tag = "projectile";

        vaseCollider.enabled = true;
    }
    #endregion
}
