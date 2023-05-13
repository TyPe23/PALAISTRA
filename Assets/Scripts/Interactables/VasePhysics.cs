using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VasePhysics : MonoBehaviour
{
    [Tooltip("Input the possible collidable items tags that can break")]
    [SerializeField] private List<string> collidables;
    [Tooltip("Drag prefabs to here to add them to the drop pool of a vase.")]
    [SerializeField] private List<GameObject> spawns;
    [Tooltip("Int - chance that an item will spawn from a vase - 1/(max-1)")]
    [SerializeField] private int max;

    public bool playerInput;
    public GameObject mesh;
    public BoxCollider trigger;
    public BoxCollider vaseCollider;
    public GameObject vasePieces;

    private PlayerStates state;
    private AudioSource soundSrc;
    private Transform parent;
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        soundSrc = GetComponent<AudioSource>();
        state = player.GetComponent<PlayerStates>();
        parent = transform.parent;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && state.state == playerStates.DASH)
        {
            breakVase();
        }
    }

    private void breakVase()
    {
        Game.globalInstance.sndPlayer.PlaySound(SoundType.POT_BREAK, soundSrc);

        vasePieces.transform.parent = parent;

        vasePieces.SetActive(true);

        int chance = Random.Range(1, max);
        if (chance == max - 1)
        {
            int spawnNumber = Random.Range(0, spawns.Count);
            Instantiate(spawns[spawnNumber], transform.position, Quaternion.identity);
        }
        StartCoroutine(waitThenDestroy());
    }

    private IEnumerator waitThenDestroy()
    {
        vaseCollider.enabled = false;
        trigger.enabled = false;
        mesh.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
