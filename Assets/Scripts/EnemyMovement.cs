using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public bool playerDetected;
    public Transform player;
    public Vector3 origin;

    public NavMeshAgent agent;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        origin= transform.position;

    }
    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0);
            agent.SetDestination(player.position);
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = new Color(0, 255, 0);
            agent.SetDestination(origin);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

}
