using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enterance : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private BoxCollider stop;
    // Start is called before the first frame update
    void Start()
    {
        stop = GetComponent<BoxCollider>();
        stop.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            animator.SetBool("trigger", true);
            stop.enabled = true;
            var pay = other.GetComponent<inputs>();
            var rm = GameObject.Find("RoomManager").GetComponent<RoomManager>().entering = false;
            pay.move = new Vector2(0, 0);
            pay.moveDir = new Vector2(0, 0);
        }
    }
}
