using Cinemachine;
using HutongGames.PlayMaker.Actions;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerEnemyInteraction : MonoBehaviour
{
    public Transform parent;
    public GameObject player;
    public Transform throwRef;
    private PlayerStates states;
    private ThirdPersonController charCon;
    public Transform attachPoint;
    private Slider slider;
    private CapsuleCollider capCollider;

    private bool attached = false;
    private Rigidbody rb;
    private bool grounded;
    private CinemachineImpulseSource shake;

    private EnemyMovement movement;
    private NavMeshAgent agent;

    [SerializeField]
    private int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        player = GameObject.FindWithTag("Player");
        throwRef = GameObject.FindWithTag("throwRef").transform;
        attachPoint = GameObject.FindWithTag("lariatAttach").transform;
        slider = gameObject.GetComponentInChildren<Slider>();
        rb = GetComponent<Rigidbody>();
        charCon = player.GetComponent<ThirdPersonController>();
        states = player.GetComponent<PlayerStates>();
        shake = player.GetComponent<CinemachineImpulseSource>();
        capCollider = player.GetComponent<CapsuleCollider>();
        movement = GetComponent<EnemyMovement>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (states.letGo && attached)
        {
            detach();

            Vector3 launch = throwRef.position - player.transform.position;
            launch.y = 0.1f;

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

        if (health <= 0)
        {
            //trigger death anim
            gameObject.tag = "Untagged";
            slider.gameObject.SetActive(false);
            movement.enabled = false;
            agent.enabled = false;
        }

        slider.value = health;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("hitbox") && gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            attach();
            movement.enabled = false;
            agent.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("floor") && !grounded)
        {
            print(collision.transform.tag);
            grounded = true;
            shake.GenerateImpulseWithForce(0.1f);
            agent.enabled = true;
            movement.enabled = true;
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);

            if (health <= 0)
            {
                rb.isKinematic = true;
                capCollider.enabled = false;
            }
        }
    }

    private void attach()
    {
        charCon.grab = true;
        transform.parent = attachPoint;
        transform.position = attachPoint.position;
        rb.useGravity = false;
        attached = true;
        rb.isKinematic = true;
    }
    
    private void detach()
    {
        StartCoroutine(pauseCollision());
        grounded = false;
        attached = false;
        states.letGo = false;
        rb.isKinematic = false;
        transform.parent = parent;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.useGravity = true;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    private IEnumerator pauseCollision()
    {
        capCollider.enabled = false;
        yield return new WaitForSeconds(0.25f);
        capCollider.enabled = true;
    }
}
