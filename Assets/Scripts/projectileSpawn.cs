using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    public GameObject projectile;
    public bool continuous;
    public float spawnTime = 1;
    private bool canSpawn = true;
    private Transform spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && continuous)
        {
            Instantiate(projectile, spawnPos.position, transform.rotation);
            StartCoroutine(spawnInterval());
        }
    }

    public void tiggered()
    {
        Instantiate(projectile, spawnPos.position, transform.rotation);
    }

    private IEnumerator spawnInterval()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        canSpawn = true;
    }
}
