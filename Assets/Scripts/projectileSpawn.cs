using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileSpawn : MonoBehaviour
{
    public GameObject projectile;
    public bool continuous;
    public float spawnTime = 1;
    private bool canSpawn = true;
    private Transform normalRef;

    // Start is called before the first frame update
    void Start()
    {
        normalRef = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && continuous)
        {
            print("spawned");
            Instantiate(projectile, transform.position + Vector3.left, transform.rotation);
            StartCoroutine(spawnInterval());
        }
    }

    public void tiggered()
    {
        Instantiate(projectile, transform.position + (Vector3.forward * 0.25f), transform.rotation);
    }

    private IEnumerator spawnInterval()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        canSpawn = true;
    }
}
