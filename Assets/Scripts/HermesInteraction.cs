using System.Collections;
using UnityEngine;

public class HermesInteraction : MonoBehaviour
{
    public bool repeat;
    public DialogueTrigger trigger;
    public DialogueManager man;

    [SerializeField] private GameObject text;
    [SerializeField] private GameObject extra;
    [SerializeField] private StarterAssetsInputs player;

    private GameObject buttons;
    private bool interactable;
    private PlayerStats ps;
    private Collider coll;

    void Start()
    {
        PlayerPrefs.SetInt("Score", 0);
        interactable = true;
        text.SetActive(false);
        ps = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        coll = GetComponent<Collider>();
        extra = GameObject.Find("Currency");
        buttons = GameObject.Find("Buttons");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //TODO enable ui above head
            text.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player.interact && other.transform.CompareTag("Player") && interactable)
        {
            coll.enabled = false;
            StartCoroutine(startDialogue());
            extra.SetActive(false);
            buttons.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //TODO disable ui
        text.SetActive(false);
        extra.SetActive(true);
        buttons.SetActive(true);
    }

    IEnumerator startDialogue()
    {
        if (repeat && interactable)
        {
            StartCoroutine(PauseInteract());
            ps.MoveSpeed = 0;
            yield return new WaitForSeconds(0.3f);
            coll.enabled = true;

            man.DisplayNextSentence();
        }
        else if(interactable)
        {
            StartCoroutine(PauseInteract());
            ps.MoveSpeed = 0;
            yield return new WaitForSeconds(0.3f);
            repeat = true;
            coll.enabled = true;

            trigger.TriggerDialogue();
        }
        //print(man.sentences.Count);
        if (man.sentences.Count == 0)
        {
            //print("Reseting trigger");
            StartCoroutine(ResetTrigger());
        }
        yield return null;
    }

    IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(10);
        if(man.sentences.Count == 0)
        {
            print("Turning repeat off");
            Destroy(gameObject.GetComponent<DialogueTrigger>());
            Destroy(text);
        }
    }

    public IEnumerator PauseInteract()
    {
        interactable = false;
        yield return new WaitForSeconds(1);
        print("Should be able to continue");
        interactable = true;
    }
}
