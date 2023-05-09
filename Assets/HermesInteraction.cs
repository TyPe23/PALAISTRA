using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermesInteraction : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs player;
    public bool repeat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO enable ui above head
    }

    private void OnTriggerStay(Collider other)
    {
        if (player.interact)
        {
            print("start dialog");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //TODO disable ui
    }
}
