using HutongGames.PlayMaker.Actions;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyInteraction : MonoBehaviour
{
    public Transform parent;
    public GameObject player;
    private PlayerStates states;
    private ThirdPersonController charCon;
    public Transform attachPoint;

    private bool attached = false;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        charCon = player.GetComponent<ThirdPersonController>();
        states = player.GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (states.letGo && attached)
        {
            detach();

            Vector3 launch = transform.position;
            launch.y = 0;

            rb.AddExplosionForce(1000, launch, 1);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        print(states.state);
        if (collision.gameObject.CompareTag("hitbox"))// && states.state == playerStates.PILEDRIVER)
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            attach();
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
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<CapsuleCollider>().enabled = true;
    }
}
