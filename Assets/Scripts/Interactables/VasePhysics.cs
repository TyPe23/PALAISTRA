using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasePhysics : MonoBehaviour
{

    [SerializeField] private List<string> collidables;
    [SerializeField] private GameObject player;
    private BoxCollider boxtrigger;
    private Rigidbody body;
    private bool sprint;

    // Start is called before the first frame update
    void Start()
    {

        boxtrigger = GetComponent<BoxCollider>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<StarterAssetsInputs>(out StarterAssetsInputs com))
        {
            sprint = com.sprint;
        }
        foreach(string possible in collidables)
        {
            if (other.transform.CompareTag(possible)&&sprint)
            {

                print("Entering Trigger");
                body.useGravity = true;
                
            }
        }
    }
}
