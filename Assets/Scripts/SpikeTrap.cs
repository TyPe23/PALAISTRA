using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField]private GameObject spike;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        spike.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.CompareTag("Player") || other.transform.CompareTag("enemy")) && !isActive)
        {
            //start trap
            StartCoroutine(startTrap());
        }
    }

    private IEnumerator startTrap()
    {
        isActive = true;
        yield return new WaitForSeconds(0.8f);
        spike.SetActive(true); 
        StartCoroutine(endTrap());
    }
    private IEnumerator endTrap()
    {
        yield return new WaitForSeconds(0.5f);
        spike.SetActive(false);
        isActive = false;
    }

}
