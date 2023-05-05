using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    private Transform player;
    private MeshRenderer mesh;
    private bool broken = false;

    public bool tracking;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mesh = GetComponentInChildren<MeshRenderer>();
        StartCoroutine(destroyAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken)
        {
            if (tracking)
            {
                Vector3 directionToTarget = player.position - transform.position;
                directionToTarget.y = 0;
                Vector3 currentDirection = transform.forward;
                currentDirection.y = 0;
                float maxTurnSpeed = 100f; // degrees per second
                Vector3 resultingDirection = Vector3.RotateTowards(currentDirection, directionToTarget, maxTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 1f);
                transform.rotation = Quaternion.LookRotation(resultingDirection);
            }
        
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
        if (!collision.transform.CompareTag("enemy") || !collision.transform.CompareTag("spawner") || !collision.transform.CompareTag("projectile"))
        {
            //particle effect
            //sound
            broken = true;
            mesh.enabled = false;
            StartCoroutine(waitThenDestroy());
        }
    }

    private IEnumerator waitThenDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    
    private IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
