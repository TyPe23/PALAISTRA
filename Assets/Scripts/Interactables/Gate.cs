using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 height;
    private bool isIn;

    [Tooltip("Determines the hightest point on the gate.")]
    [SerializeField] private int peak;
    
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        height = new Vector3(origin.x, origin.y + peak, origin.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y > origin.y && !isIn)
        { gameObject.transform.position = Vector3.MoveTowards(transform.position, origin, 0.05f); }
    }

    private void OnTriggerEnter(Collider other)
    {
        isIn = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (transform.position.y < height.y)
            {
                gameObject.transform.position = Vector3.MoveTowards(transform.position,height, 0.05f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isIn = false;
    }
}
