using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasePhysics : MonoBehaviour
{
    [Tooltip("Input the possible collidable items tags that can break")]
    [SerializeField] private List<string> collidables;
    [Tooltip("Drag prefabs to here to add them to the drop pool of a vase.")]
    [SerializeField] private List<GameObject> spawns;
    [Tooltip("Int - chance that an item will spawn from a vase - 1/(max-1)")]
    [SerializeField] private int max;
    public bool playerInput;

    private void OnTriggerEnter(Collider other)
    {
        print(other.transform.tag);

        if(other.TryGetComponent<PlayerStates>(out PlayerStates com))
        {
            playerInput = (com.state == playerStates.DASH || com.hitbox.enabled)  ? true : false;
        }
        foreach(string possible in collidables)
        {
            if (other.transform.CompareTag(possible))
            {
                int chance = Random.Range(1, max);
                if(chance == max-1) {
                    int spawnNumber = Random.Range(0, spawns.Count);
                    Instantiate(spawns[spawnNumber], transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }
}
