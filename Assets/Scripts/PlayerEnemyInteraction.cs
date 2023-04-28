using HutongGames.PlayMaker.Actions;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyInteraction : MonoBehaviour
{
    public Transform parent;
    private Rigidbody rb;
    public Transform attachPoint;
    public ThirdPersonController charCon;
    private bool attached = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (charCon.letGo && attached)
        {
            detach();
            rb.AddForce(new Vector3(0, 100f, -500f));
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("hitbox") && !charCon.grab)
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
        print(parent.name);
        attached = false;
        charCon.letGo = false;
        rb.isKinematic = false;
        transform.parent = parent;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.useGravity = true;
        charCon.grab = false;
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
