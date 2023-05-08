using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinSpawn : MonoBehaviour
{
    [SerializeField] private int worth;
    private Collider collision;
    private AudioSource soundSrc;
    private void Awake()
    {
        collision = GetComponent<Collider>();
        soundSrc = GetComponent<AudioSource>();
        collision.enabled = false;
    }
    void Start()
    {

        StartCoroutine(waitAfterSpawn());
        
    }

    IEnumerator waitAfterSpawn()
    {
        yield return new WaitForSeconds(3);
        collision.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("You earned " + worth);
            GameObject.Find("Player").GetComponent<PlayerStats>().currency += worth;
            //TODO add money to character inventory
            Game.globalInstance.sndPlayer.PlaySound(SoundType.COIN, soundSrc);
            Destroy(gameObject);
        }
    }
}
