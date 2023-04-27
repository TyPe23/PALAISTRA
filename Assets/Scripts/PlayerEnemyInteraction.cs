using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyInteraction : MonoBehaviour
{
    private Transform tempTransform;
    private Rigidbody rb;
    public Transform attachPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("hitbox"))
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            attach();
        }
    }

    private void attach()
    {
        tempTransform = transform;
        transform.parent = attachPoint;
        transform.position = attachPoint.position;
        rb.useGravity = false;
        rb.isKinematic = true;
    }
    
    private void dettach()
    {
        transform.parent = tempTransform;
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
