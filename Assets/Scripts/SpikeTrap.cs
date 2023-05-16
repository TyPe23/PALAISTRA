using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpikeTrap : MonoBehaviour
{
    [SerializeField]private GameObject spikes;
    private SphereCollider hitbox;
    private AudioSource soundSrc;

    private bool isActive = false;
    private bool prime;
    private bool startSpikes;
    private bool endSpikes;

    private Vector3 primePos;
    private Vector3 startPos;
    private Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponentInChildren<SphereCollider>();
        soundSrc = GetComponent<AudioSource>();

        hitbox.enabled = false;

        endPos = spikes.transform.position;

        primePos = spikes.transform.position;
        primePos.y += 0.75f;

        startPos = spikes.transform.position;
        startPos.y += 2.5f;
    }

    private void Update()
    {
        if (prime)
        {
            spikes.transform.Translate((primePos - spikes.transform.position) * Time.deltaTime * 6);
        }
        else if (startSpikes)
        {
            spikes.transform.Translate((startPos - spikes.transform.position) * Time.deltaTime * 10);
        }
        else if (endSpikes)
        {
            spikes.transform.Translate((endPos - spikes.transform.position) * Time.deltaTime * 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.CompareTag("Player") || other.transform.CompareTag("enemyNoHit")) && !isActive)
        {
            //start trap
            StartCoroutine(primeTrap());
        }
    }

    private IEnumerator primeTrap()
    {

        endSpikes = false;
        isActive = true;
        prime = true;
        Game.globalInstance.sndPlayer.PlaySound(SoundType.SPIKE_PRIME, soundSrc);
        yield return new WaitForSeconds(1f);
        StartCoroutine(startTrap());
    }
    private IEnumerator startTrap()
    {
        Game.globalInstance.sndPlayer.PlaySound(SoundType.SPIKE, soundSrc);
        isActive = true;
        prime = false;
        startSpikes = true;
        hitbox.enabled = true;
        yield return new WaitForSeconds(1f);
        StartCoroutine(endTrap());
    }
    
    private IEnumerator endTrap()
    {
        startSpikes = false;
        hitbox.enabled = false;
        endSpikes = true;
        yield return new WaitForSeconds(0.5f);
        isActive = false;
    }

}
