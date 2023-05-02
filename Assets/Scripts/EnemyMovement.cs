using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public bool playerDetected;
    public Transform player;
    public int speed;

    public NavMeshAgent agent;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }
    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            print("go to player");
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position);
            print("No player");
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
