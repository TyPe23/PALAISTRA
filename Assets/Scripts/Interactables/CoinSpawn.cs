using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinSpawn : MonoBehaviour
{
    [SerializeField] private int worth;
    private Collider collision;
    private AudioSource soundSrc;
    private MeshRenderer mesh;
    private void Awake()
    {
        collision = GetComponent<Collider>();
        soundSrc = GetComponent<AudioSource>();
        mesh = GetComponentInChildren<MeshRenderer>();
        collision.enabled = false;
    }
    void Start()
    {

        StartCoroutine(waitAfterSpawn());
        
    }

    private void Update()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(rot.x, rot.y + Time.deltaTime * 1000, rot.z);
    }

    IEnumerator waitAfterSpawn()
    {
        yield return new WaitForSeconds(0.5f);
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
            StartCoroutine(waitToDestroy());
        }
    }

    IEnumerator waitToDestroy()
    {
        collision.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
