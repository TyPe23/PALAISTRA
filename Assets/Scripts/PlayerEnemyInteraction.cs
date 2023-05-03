using Cinemachine;
using HutongGames.PlayMaker.Actions;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnemyInteraction : MonoBehaviour
{
    public Transform parent;
    public GameObject player;
    public Transform throwRef;
    private PlayerStates states;
    private ThirdPersonController charCon;
    public Transform attachPoint;
    public Slider slider;
    private CapsuleCollider capCollider;

    private bool attached = false;
    private Rigidbody rb;
    private bool grounded;
    private CinemachineImpulseSource shake;

    [SerializeField]
    private int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        charCon = player.GetComponent<ThirdPersonController>();
        states = player.GetComponent<PlayerStates>();
        shake = player.GetComponent<CinemachineImpulseSource>();
        capCollider = player.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (states.letGo && attached)
        {
            detach();

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

        if (health <= 0)
        {
            //trigger death anim
            gameObject.tag = "Untagged";
            slider.gameObject.SetActive(false);
        }

        slider.value = health;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("hitbox") && gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            attach();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("floor") && !grounded)
        {
            grounded = true;
            shake.GenerateImpulseWithForce(0.1f);

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
        attached = false;
        states.letGo = false;
        rb.isKinematic = false;
        transform.parent = parent;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.useGravity = true;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        StartCoroutine(pauseCollision());
    }

    private IEnumerator pauseCollision()
    {
        capCollider.enabled = false;
        yield return new WaitForSeconds(0.2f);
        capCollider.enabled = true;
    }
}
