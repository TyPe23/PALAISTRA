using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HermesInteraction : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs player;
    public bool repeat;
    public DialogueTrigger trigger;
    public DialogueManager man;
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
        if (player.interact&&other.transform.CompareTag("Player"))
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(startDialogue()); 
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //TODO disable ui
    }

    IEnumerator startDialogue()
    {
        if (repeat)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerStats>().MoveSpeed = 0;
            yield return new WaitForSeconds(0.3f);
            gameObject.GetComponent<CapsuleCollider>().enabled = true;

            man.DisplayNextSentence();
            
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerStats>().MoveSpeed = 0;
            yield return new WaitForSeconds(0.3f);
            repeat = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = true;

            trigger.TriggerDialogue();
        }
    }

}
